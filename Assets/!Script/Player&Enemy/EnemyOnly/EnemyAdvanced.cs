using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyAdvanced_Special_MoveCycle : Enemy
{
    [Header("複数の攻撃パターンを同時使用")]
    [SerializeField] private List<AttackSet> attackSets = new List<AttackSet>();

 /*   [Header("特殊攻撃パターン（HP半分以下で1回だけ）")]
    [SerializeField] private AttackSet specialAttack;
    public int specialAttackDamage = 3;

    [Header("特殊攻撃の前後待機(秒)")]
    [SerializeField] private float preSpecialWait = 1.0f;
    [SerializeField] private float postSpecialWait = 2.0f;
 */
    [Header("停止するY座標（これ以下で停止）")]
    [SerializeField] private float stopPosY = 2f;

    [Header("最初の降下スピード")]
    [SerializeField] private float firstFallSpeed = 2f;

    [Header("停止している時間（秒）")]
    [SerializeField] private float stopDuration = 3f;

    [Header("再降下時のスピード")]
    [SerializeField] private float secondFallSpeed = 4f;

    private EnemyHealth _health;
    private bool didSpecial = false;
    private bool inSpecial = false;
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
        }

        foreach (var set in attackSets)
        {
            set.shootTimer = set.initialDelay;

            if (set.bulletPrefab != null)
                BulletPool.Instance.RegisterBulletPrefab(set.bulletPrefab, set.poolSize);
        }

        _rb.velocity = Vector2.down * firstFallSpeed;

        // 非同期処理を起動
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
    // 攻撃処理
    // =========================================================
    protected override void _Attack()
    {
        if (!_bAttack || inSpecial || !_initialized) return;
        if (attackSets == null || attackSets.Count == 0) return;

        foreach (var set in attackSets)
        {
            if (set.attackPattern == null || set.bulletPrefab == null) continue;

            set.shootTimer += Time.deltaTime;

            if (set.shootTimer >= Mathf.Max(0.05f, set.shootInterval))
            {
                set.attackPattern.Shoot(
                    this.gameObject.transform.position,
                    set.shootAngle,
                    set.bulletPrefab,
                    set.damage
                );
                set.shootTimer = 0f;
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

          /*  if (hpPercent < 0.5f && !didSpecial)
            {
                didSpecial = true;
                await SpecialAttackRoutineAsync(token);
            }
         */
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.2f), cancellationToken: token);
        }
    }

    // =========================================================
    // 特殊攻撃（UniTask版）
    // =========================================================
  /*  private async UniTask SpecialAttackRoutineAsync(CancellationToken token)
    {
        inSpecial = true;
        _rb.velocity = Vector2.zero;
        Debug.Log($"{name}：特殊攻撃準備中...");

        await UniTask.Delay(System.TimeSpan.FromSeconds(preSpecialWait), cancellationToken: token);

        if (specialAttack != null)
        {
            Debug.Log($"{name}：特殊攻撃発動！！！");
            specialAttack.attackPattern.Shoot(
                this.gameObject.transform.position,
                specialAttack.shootAngle,
                specialAttack.bulletPrefab,
                specialAttackDamage
            );
        }

        await UniTask.Delay(System.TimeSpan.FromSeconds(postSpecialWait), cancellationToken: token);

        Debug.Log($"{name}：特殊攻撃終了、通常攻撃再開");

        if (hasRestarted)
            _rb.velocity = Vector2.down * secondFallSpeed;

        inSpecial = false;
    }*/
}
