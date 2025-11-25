using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyPhaseAttack : MonoBehaviour
{
    [Header("攻撃フェーズのリスト")]
    [SerializeField] private List<AttackPhase> phases = new();

    private EnemyHealth _health;
    private CancellationTokenSource _cts = new CancellationTokenSource();
    private int currentPhaseIndex = 0;
    private bool _initialized = false;
    public event System.Action<int> OnPhaseChanged;

    // 攻撃してよいか（画面内フラグ）
    protected bool _bAttack;

    private void Start()
    {
        _health = GetComponent<EnemyHealth>();
        if (_health == null)
        {
            Debug.LogError($"{name} に EnemyHealth がありません！");
            return;
        }

        // AttackSet の BulletPool 登録
        foreach (var phase in phases)
        {
            foreach (var set in phase.attackSets)
            {
                set.shootTimer = -set.initialDelay;
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


    // -------------------------------------------------------------
    // 初期化
    // -------------------------------------------------------------


    private void OnDestroy()
    {
        _cts?.Cancel();
    }

  
    // -------------------------------------------------------------
    // フェーズ管理ループ
    // -------------------------------------------------------------
    private async UniTaskVoid MultiPhaseAttackLoopAsync(CancellationToken token)
    {
        await UniTask.WaitUntil(() => _initialized && _bAttack && _health != null, cancellationToken: token);

        while (!token.IsCancellationRequested)
        {
            if (currentPhaseIndex >= phases.Count)
                break; // すべてのフェーズが終了

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

                //イベントを起こしている
                OnPhaseChanged?.Invoke(currentPhaseIndex);

                continue;
            }

            await UniTask.Yield();
        }
    }

    // -------------------------------------------------------------
    // AttackSet の処理（Enemy の物を流用）
    // -------------------------------------------------------------
    private void HandleAttackSet(AttackSet set)
    {
        if (set.attackPattern == null || set.bulletPrefab == null)
            return;

        set.shootTimer += Time.deltaTime;

        if (set.shootTimer >= Mathf.Max(0.05f, set.shootInterval))
        {
            set.attackPattern.Shoot(
                transform.position,
                set.shootAngle,
                set.bulletPrefab,
                set.damage
            );

            set.shootTimer = 0f;
        }
    }
    // 確実に動作する方法
    private void OnBecameVisible() { _bAttack = true; }
    private void OnBecameInvisible() { _bAttack = false; }

}
