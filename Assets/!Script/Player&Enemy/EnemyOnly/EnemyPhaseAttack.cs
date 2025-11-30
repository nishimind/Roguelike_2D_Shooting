using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyPhaseAttack : MonoBehaviour
{
    [Header("攻撃フェーズのリスト")]
    [SerializeField] private List<AttackPhase> phases = new();

    private EnemyHealth _health;

    // フェーズ管理用のキャンセルトークン
    private CancellationTokenSource _cts = new CancellationTokenSource();

    // AttackPattern 側が「進行中攻撃のキャンセル」に使いたい場合用のトークン
    public CancellationToken AttackToken => _cts.Token;

    private int currentPhaseIndex = 0;
    private bool _initialized = false;

    public event System.Action<int> OnPhaseChanged;

    // 攻撃してよいか（画面内フラグ）
    protected bool _bAttack;

    // 最後に撃つ一発（死亡時攻撃）用
    public List<AttackSet> LastAttacks = new List<AttackSet>();

    private void Start()
    {
        _health = GetComponent<EnemyHealth>();
        if (_health == null)
        {
            Debug.LogError($"{name} に EnemyHealth がありません！");
            return;
        }

        // 死亡イベント購読
        _health.OnDeath.AddListener(OnEnemyDeath);

        // AttackSet の BulletPool 登録 ＋ タイマー系の初期化
        foreach (var phase in phases)
        {
            foreach (var set in phase.attackSets)
            {
                // ★ 初期化：タイマーとバースト状態
                set.shootTimer = 0f;
                set.firstShotDone = false;
                set.currentShotCount = 0;
                set.inBurstCooldown = false;
                set.burstTimer = 0f;

                if (set.bulletPrefab != null)
                {
                    BulletPool.Instance.RegisterBulletPrefab(set.bulletPrefab, set.poolSize);
                }
            }
        }

        _initialized = true;

        // フェーズ攻撃ループを開始
        MultiPhaseAttackLoopAsync(_cts.Token).Forget();
    }

    private void OnDestroy()
    {
        if (!_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }
    }

    // -------------------------------------------------------------
    // フェーズ管理ループ
    // -------------------------------------------------------------
    private async UniTaskVoid MultiPhaseAttackLoopAsync(CancellationToken token)
    {
        // 初期化完了 & 画面内 & HPコンポーネントあり を待つ
        await UniTask.WaitUntil(
            () => _initialized && _bAttack && _health != null,
            cancellationToken: token
        );

        while (!token.IsCancellationRequested)
        {
            if (currentPhaseIndex >= phases.Count)
                break; // すべてのフェーズが終了

            // 画面外に出てる間は攻撃しない
            if (!_bAttack)
            {
                await UniTask.Yield();
                continue;
            }

            var phase = phases[currentPhaseIndex];

            // HP％計算
            float hpPercent = (_health.currentHP * 100f) / _health.maxHP;
            phase.timer += Time.deltaTime;

            // --- 攻撃実行 ---
            foreach (var set in phase.attackSets)
            {
                HandleAttackSet(set);
            }

            // --- フェーズ終了条件チェック ---
            bool timeCondition = (phase.durationSeconds > 0f && phase.timer >= phase.durationSeconds);
            bool hpCondition = (phase.nextPhaseHpPercent >= 0f && hpPercent <= phase.nextPhaseHpPercent);

            if (timeCondition || hpCondition)
            {
                currentPhaseIndex++;

                // フェーズ変更イベント
                OnPhaseChanged?.Invoke(currentPhaseIndex);

                continue;
            }

            await UniTask.Yield();
        }
    }

    // -------------------------------------------------------------
    // AttackSet の処理（初回遅延＋通常インターバル＋バースト対応）
    // -------------------------------------------------------------
    private void HandleAttackSet(AttackSet set)
    {
        if (set.attackPattern == null || set.bulletPrefab == null)
            return;

        // ---- ① バーストインターバル中なら、タイマー進めるだけ ----
        if (set.inBurstCooldown)
        {
            set.burstTimer += Time.deltaTime;

            if (set.burstTimer >= set.burstInterval)
            {
                // インターバル終了 → 通常状態に戻す
                set.inBurstCooldown = false;
                set.burstTimer = 0f;
                set.currentShotCount = 0;
            }

            return; // インターバル中は撃たない
        }

        // ---- ② 通常の発射タイマー ----
        set.shootTimer += Time.deltaTime;

        // 初回のみ initialDelay、それ以降は shootInterval
        float targetInterval = set.firstShotDone
            ? set.shootInterval
            : set.initialDelay;

        targetInterval = Mathf.Max(0.05f, targetInterval);

        if (set.shootTimer >= targetInterval)
        {
            // ---- 弾発射 ----
            // ★ ここで AttackPattern 側が AttackToken を使いたい場合は
            //    AttackPatternSO に SetToken(AttackToken) を持たせて事前に渡す、などの拡張が可能。
            set.attackPattern.Shoot(
                transform.position,
                set.shootAngle,
                set.bulletPrefab,
                set.damage
            );

            set.shootTimer = 0f;
            set.firstShotDone = true;
            set.currentShotCount++;

            // ---- ③ バースト回数チェック ----
            if (set.burstCount > 0 && set.currentShotCount >= set.burstCount)
            {
                // バースト上限に達したのでインターバル開始
                set.inBurstCooldown = true;
                set.burstTimer = 0f;
            }
        }
    }

    // -------------------------------------------------------------
    // 死亡時のラストアタック
    // -------------------------------------------------------------
    private void OnEnemyDeath()
    {
        Debug.Log($"{name} が死んだので LastAttack 発動！");

        // メイン攻撃ループを止める
        if (!_cts.IsCancellationRequested)
            _cts.Cancel();

        // 即時、LastAttacks を撃つ
        FireLastAttacksAsync().Forget();
    }

    private async UniTaskVoid FireLastAttacksAsync()
    {
        foreach (var set in LastAttacks)
        {
            if (set.attackPattern == null || set.bulletPrefab == null)
                continue;

            set.attackPattern.Shoot(
                transform.position,
                set.shootAngle,
                set.bulletPrefab,
                set.damage
            );

            // ちょっとだけ間を空けたい場合
            await UniTask.Delay(50);
        }
    }

    // 画面内/外で攻撃許可を切り替え
    private void OnBecameVisible() { _bAttack = true; }
    private void OnBecameInvisible() { _bAttack = false; }

    // =============================================================
    //  トークンなどから「攻撃を完全停止」させるための入口
    // =============================================================
    public void ForceStopAttack()
    {
        // 新しく撃たせない
        _bAttack = false;

        // フェーズ管理ループ & AttackToken をキャンセル
        if (!_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }
    }
}