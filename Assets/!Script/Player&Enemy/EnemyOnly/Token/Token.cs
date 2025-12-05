using UnityEngine;

public class TokenHealth : EnemyHealth
{
    [Header("通常時の色")]
    [SerializeField] private Color activeColor = Color.white;

    [Header("死亡時の薄い色")]
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);

    [Header("HP0になったら攻撃停止させる砲台（複数可）")]
    [SerializeField] private EnemyAdvanced_Special_MoveCycle[] controlledTowers;

    private Collider2D[] _colliders;
    private bool _isDown = false;

    protected override void Start()
    {
        base.Start();

        // Collider 初期化
        _colliders = GetComponentsInChildren<Collider2D>();

        // Token は基本攻撃しないので、外見だけセット
        ApplyState(true);
    }

    // =============================
    //     ダメージ処理
    // =============================
    protected override void Update()
    {
        if (_isDown) return;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    // =============================
    //         死亡処理
    // =============================
    protected override void Die()
    {
        if (_isDown) return;
        _isDown = true;

        // 点滅など EnemyHealth のコルーチンを全停止
        StopAllCoroutines();

        // ▼ 砲台の攻撃を止める ▼
        if (controlledTowers != null && controlledTowers.Length > 0)
        {
            foreach (var tower in controlledTowers)
            {
                if (tower != null)
                {
                    tower.ForceStopAttack();
                    Debug.Log($"トークン: {tower.name} の攻撃を停止しました");
                }
            }
        }

        // ▼ アイテムドロップやイベント呼び出し（EnemyHealth の機能） ▼
        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();

        // ボス戦用：最後の敵処理
        if (isLastEnemy)
            Siene_Change_Main_Shooting.Instance.lastEnemyDead = true;

        // 敵リストから削除（倒した扱い）
        Siene_Change_Main_Shooting.Instance.UnregisterEnemy(this.gameObject);

        OnDeath?.Invoke();

        // ▼ Destroy せずに無力化 ▼
        ApplyState(false);
    }

    // =============================
    //     トークン復活（任意）
    // =============================
    public void Revive()
    {
        if (!_isDown) return;

        _isDown = false;
        currentHP = maxHP;

        // 元の状態に戻す
        ApplyState(true);

        // 敵リストへ再登録
        Siene_Change_Main_Shooting.Instance.RegisterEnemy(this.gameObject);

        // ★砲台の攻撃再開処理は必要ならここで追加する（今は停止したまま）
    }

    // =============================
    //    見た目 ＆ 当たり判定
    // =============================
    private void ApplyState(bool active)
    {
        // コライダー ON/OFF
        if (_colliders != null)
        {
            foreach (var col in _colliders)
            {
                if (col != null)
                    col.enabled = active;
            }
        }

        // 色変更（EnemyHealth の機能利用）
        if (_renderers != null)
        {
            if (active)
            {
                // 元の色へ
                for (int i = 0; i < _renderers.Length; i++)
                {
                    if (_renderers[i] != null)
                        _renderers[i].color = _originalColors[i];
                }
            }
            else
            {
                // 無力化した薄い色へ
                foreach (var r in _renderers)
                {
                    if (r != null)
                        r.color = inactiveColor;
                }
            }
        }
    }
}