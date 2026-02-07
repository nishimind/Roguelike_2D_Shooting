using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyAdvanced_Special_MoveCycle : Enemy
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
    private bool didSpecial = false;   // 今は使ってないけど、後で復活させるならそのまま残し
    private bool inSpecial = false;    // 同上
    private bool hasStopped = false;
    private bool hasRestarted = false;
    private bool _initialized = false;
    private AttackEffect _attackEffect;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    // =========================================================
    // 初期化
    // =========================================================
    protected override void _Initialize()
    {
        base._Initialize();

        _attackEffect = GetComponent<AttackEffect>();

        _health = GetComponent<EnemyHealth>();
        if (_health == null)
        {
            Debug.LogError($"{name} に EnemyHealth がアタッチされていません！");
        }

        // 攻撃タイマー類の初期化＆プール登録
        foreach (var set in attackSets)
        {
            // ★ここがポイント：タイマーは0スタート、最初の1発フラグもリセット
            set.shootTimer = 0f;
            set.firstShotDone = false;

            if (set.bulletPrefab != null)
            {
                BulletPool.Instance.RegisterBulletPrefab(set.bulletPrefab, set.poolSize);
            }
        }

        // 最初はゆっくり降下
        _rb.velocity = Vector2.down * firstFallSpeed;

        // HP監視の非同期処理を起動
        HealthWatcherAsync(_cts.Token).Forget();
        _initialized = true;
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
            Debug.Log($"{name}：停止位置に到達（Y={stopPosY}）");

            RestartMoveAfterDelayAsync(_cts.Token).Forget();
        }
    }

    private async UniTaskVoid RestartMoveAfterDelayAsync(CancellationToken token)
    {
        Debug.Log($"{name}：{stopDuration}秒間停止中...");
        await UniTask.Delay(System.TimeSpan.FromSeconds(stopDuration), cancellationToken: token);

        if (token.IsCancellationRequested) return;

        hasRestarted = true;
        _rb.velocity = Vector2.down * secondFallSpeed;
        Debug.Log($"{name}：再降下開始（速度={secondFallSpeed}）");
    }

    // =========================================================
    // 攻撃処理（初回だけ initialDelay → 以降は shootInterval）
    // =========================================================
    protected override void _Attack()
    {
        if (!_bAttack || !_initialized) return;

        foreach (var set in attackSets)
        {
            if (set.attackPattern == null || set.bulletPrefab == null)
                continue;

            set.shootTimer += Time.deltaTime;

            float interval = set.firstShotDone
                ? set.shootInterval
                : set.initialDelay;

            if (set.shootTimer >= interval)
            {
                // ▼ 発射前：赤点滅演出 ▼
                _attackEffect?.PlayAsync().Forget();

                // ▼ レーザー発射 ▼
                set.attackPattern.Shoot(
                    transform.position,
                    set.shootAngle,
                    set.bulletPrefab,
                    (int)set.damage
                );

                set.shootTimer = 0f;
                set.firstShotDone = true;
            }
        }
    }

    // =========================================================
    // HP監視処理（UniTask版）
    // =========================================================
    private async UniTaskVoid HealthWatcherAsync(CancellationToken token)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.3f), cancellationToken: token);

        while (!token.IsCancellationRequested)
        {
            if (_health == null || _health.maxHP <= 0)
            {
                await UniTask.Yield(token);
                continue;
            }

            float hpPercent = (float)_health.currentHP / _health.maxHP;

            // 特殊攻撃は今コメントアウト中
            // if (hpPercent < 0.5f && !didSpecial) { ... }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.2f), cancellationToken: token);
        }
    }
    public void ForceStopAttack()
    {
        _bAttack = false;

        // 発射前演出も止める
        _attackEffect?.ForceStop();

        // 発射中レーザーを即停止
        ForceStopAllLasers();

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        Debug.Log($"{name}：攻撃とレーザーを完全停止");
    }

    public void ForceStopAllLasers()
    {
        var lasers = GetComponentsInChildren<LaserNormal>(true);

        foreach (var laser in lasers)
        {
            if (laser != null)
                laser.ForceStopLaser();
        }
    }
}