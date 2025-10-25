using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvanced_Special_Stop : Enemy
{
    [System.Serializable]
    public class AttackSet
    {
        [Header("攻撃パターン（ScriptableObject）")]
        public AttackPatternSO attackPattern;

        [Header("発射間隔(秒)")]
        public float shootInterval = 1f;

        [Header("最初の発射までの遅延(秒)")]
        public float initialDelay = 0f;

        [HideInInspector] public float shootTimer = 0f;
    }

    [Header("複数の攻撃パターンを同時使用")]
    [SerializeField] private List<AttackSet> attackSets = new List<AttackSet>();

    [Header("特殊攻撃パターン（HP半分以下で1回だけ）")]
    [SerializeField] private AttackPatternSO specialAttack;

    [Header("特殊攻撃の前後待機(秒)")]
    [SerializeField] private float preSpecialWait = 1.0f;
    [SerializeField] private float postSpecialWait = 2.0f;

    [Header("停止するY座標（これ以下で停止）")]
    [SerializeField] private float stopPosY = 2f;

    private EnemyHealth _health;
    private bool didSpecial = false;
    private bool inSpecial = false;
    private bool hasStopped = false;
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

        // 攻撃タイマー初期化
        foreach (var set in attackSets)
        {
            set.shootTimer = set.initialDelay;
        }

        StartCoroutine(HealthWatcher());
        _initialized = true;
    }

    // =========================================================
    // 移動処理（stopPosYで停止）
    // =========================================================
    protected override void _Move()
    {
        if (!_initialized || hasStopped) return;

        if (transform.position.y <= stopPosY)
        {
            _rb.velocity = Vector2.zero;
            hasStopped = true;
            Debug.Log($"{name}：停止位置に到達（Y={stopPosY}）");
        }
        else
        {
            base._Move(); // Enemy.cs の標準移動（下方向）
        }
    }

    // =========================================================
    // 攻撃処理（移動中でも撃つ）
    // =========================================================
    protected override void _Attack()
    {
        if (!_bAttack || inSpecial || !_initialized) return;
        if (attackSets == null || attackSets.Count == 0) return;

        foreach (var set in attackSets)
        {
            if (set.attackPattern == null) continue;

            set.shootTimer += Time.deltaTime;

            if (set.shootTimer >= Mathf.Max(0.05f, set.shootInterval))
            {
                set.attackPattern.Shoot(this);
                set.shootTimer = 0f;
                // Debug.Log($"{name}：{set.attackPattern.name} 発射");
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
            specialAttack.Shoot(this);
        }

        yield return new WaitForSeconds(postSpecialWait);

        Debug.Log($"{name}：特殊攻撃終了、通常攻撃再開");
        inSpecial = false;
    }
}