using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyHpVer : Enemy
{
    [Header("複数の攻撃パターンを同時使用")]
    [SerializeField] private List<AttackSet> attackSets = new List<AttackSet>();

    [Header("停止するY座標（これ以下で停止）")]
    [SerializeField] private float stopPosY = 2f;

    [Header("最初の降下スピード")]
    [SerializeField] private float firstFallSpeed = 2f;

    [Header("停止している時間（秒）")]
    [SerializeField] private float stopDuration = 3f;

    [Header("再降下時のスピード")]
    [SerializeField] private float secondFallSpeed = 4f;

    private EnemyHealth _health;
    private bool hasStopped = false;
    private bool hasRestarted = false;
    private bool _initialized = false;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    // =========================================================
    // 初期化
    // =========================================================
    protected override void _Initialize()
    {
        base._Initialize();

        _health = GetComponent<EnemyHealth>();
        if (_health == null)
        {
            Debug.LogError($"{name} に EnemyHealth がアタッチされていません！");
            return;
        }

        // 各パターンの初期設定と弾登録
        foreach (var set in attackSets)
        {
            set.shootTimer = set.initialDelay;
            if (set.bulletPrefab != null)
                BulletPool.Instance.RegisterBulletPrefab(set.bulletPrefab, set.poolSize);
        }

        _rb.velocity = Vector2.down * firstFallSpeed;
        _initialized = true;

        // 攻撃ループを開始
        MultiPatternAttackLoopAsync(_cts.Token).Forget();
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
    }

    // =========================================================
    // 移動制御
    // =========================================================
    protected override void _Move()
    {
        if (!_initialized) return;
        if (hasRestarted) return;

        if (!hasStopped && transform.position.y <= stopPosY)
        {
            hasStopped = true;
            _rb.velocity = Vector2.zero;

            RestartMoveAfterDelayAsync(_cts.Token).Forget();
        }
    }

    private async UniTaskVoid RestartMoveAfterDelayAsync(CancellationToken token)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(stopDuration), cancellationToken: token);
        if (token.IsCancellationRequested) return;

        hasRestarted = true;
        _rb.velocity = Vector2.down * secondFallSpeed;
    }

    // =========================================================
    // 攻撃は Update では行わない
    // =========================================================
    protected override void _Attack() { }

    // =========================================================
    // HPに応じて複数パターンを同時管理するループ
    // =========================================================
    private async UniTaskVoid MultiPatternAttackLoopAsync(CancellationToken token)
    {
        // 初期化完了 & カメラに映るまで待機
        await UniTask.WaitUntil(() => _bAttack && _initialized && _health != null, cancellationToken: token);

        while (!token.IsCancellationRequested)
        {
            if (_health == null || _health.maxHP <= 0)
            {
                await UniTask.Yield();
                continue;
            }

            float hpPercent = (_health.currentHP * 100f) / _health.maxHP;
        //    Debug.Log($"{name} の現在HP%: {hpPercent}% {_health.currentHP}/{_health.maxHP}");
            // 各AttackSetを独立して評価
            foreach (var set in attackSets)
            {
                if (set.attackPattern == null || set.bulletPrefab == null) continue;

                // HPが範囲内なら発射
                if (hpPercent <= set.startHP && hpPercent >= set.endHP)
                {
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
                else
                {
                    // HP範囲外ならタイマーをリセット
                    set.shootTimer = set.initialDelay;
                }
            }

            await UniTask.Yield(); // 毎フレーム更新
        }
    }
}
