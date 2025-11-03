using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvanced_Special_MoveCycle : Enemy
{
 
   

    [Header("複数の攻撃パターンを同時使用")]
    [SerializeField] private List<AttackSet> attackSets = new List<AttackSet>();

    [Header("特殊攻撃パターン（HP半分以下で1回だけ）")]
    [SerializeField] private AttackSet specialAttack;

    [Header("特殊攻撃の前後待機(秒)")]
    [SerializeField] private float preSpecialWait = 1.0f;
    [SerializeField] private float postSpecialWait = 2.0f;

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

        // 攻撃タイマー初期化 & 弾プレハブ登録
        foreach (var set in attackSets)
        {
            set.shootTimer = set.initialDelay;

            if (set.bulletPrefab != null)
              BulletPool.Instance.RegisterBulletPrefab(set.bulletPrefab);
        }

        // 最初はゆっくり降下
        _rb.velocity = Vector2.down * firstFallSpeed;

        StartCoroutine(HealthWatcher());
        _initialized = true;
    }

    // =========================================================
    // 移動制御
    // =========================================================
    protected override void _Move()
    {
        if (!_initialized) return;

        // 一度停止したあとに再降下していれば、_Move制御はしない（velocity手動制御）
        if (hasRestarted) return;

        // stopPosY 到達で停止
        if (!hasStopped && transform.position.y <= stopPosY)
        {
            hasStopped = true;
            _rb.velocity = Vector2.zero;
            Debug.Log($"{name}：停止位置に到達（Y={stopPosY}）");

            // 一定時間後に再降下開始
            StartCoroutine(RestartMoveAfterDelay());
        }
    }

    // 一定時間後に再降下を開始
    private IEnumerator RestartMoveAfterDelay()
    {
        Debug.Log($"{name}：{stopDuration}秒間停止中...");
        yield return new WaitForSeconds(stopDuration);

        hasRestarted = true;
        _rb.velocity = Vector2.down * secondFallSpeed;
        Debug.Log($"{name}：再降下開始（速度={secondFallSpeed}）");
    }

    // =========================================================
    // 攻撃処理（移動中も撃つ）
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
                set.attackPattern.Shoot(this, set.bulletPrefab);
                set.shootTimer = 0f;
            }
        }
    }


    // =========================================================
    // 特殊攻撃監視（HP50%未満で1度だけ）
    // =========================================================
    private IEnumerator HealthWatcher()
    {
        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            if (_health == null || _health.maxHP <= 0)
            {
                yield return null;
                continue;
            }

            float hpPercent = (float)_health.currentHP / _health.maxHP;

            if (hpPercent < 0.5f && !didSpecial)
            {
                didSpecial = true;
                StartCoroutine(SpecialAttackRoutine());
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    // =========================================================
    // 特殊攻撃（HP50%未満時に1度だけ発動）
    // =========================================================
    private IEnumerator SpecialAttackRoutine()
    {
        inSpecial = true;
        _rb.velocity = Vector2.zero;
        Debug.Log($"{name}：特殊攻撃準備中...");

        yield return new WaitForSeconds(preSpecialWait);

        if (specialAttack != null)
        {
            Debug.Log($"{name}：特殊攻撃発動！！！");
            specialAttack.attackPattern. Shoot(this, specialAttack.bulletPrefab);
        }

        yield return new WaitForSeconds(postSpecialWait);

        Debug.Log($"{name}：特殊攻撃終了、通常攻撃再開");

        // 特殊攻撃が終わったら、再降下してるなら再び動かす
        if (hasRestarted)
            _rb.velocity = Vector2.down * secondFallSpeed;

        inSpecial = false;
    }
}
