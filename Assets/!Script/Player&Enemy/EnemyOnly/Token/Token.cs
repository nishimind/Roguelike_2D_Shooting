using UnityEngine;

public class TokenHealth : EnemyHealth
{
    [Header("通常時の色")]
    [SerializeField] private Color activeColor = Color.white;

    [Header("死亡時の薄い色")]
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);

    private Collider2D[] _colliders;
    private bool _isDown = false;

    protected override void Start()
    {
        base.Start();

        // Collider 初期化
        _colliders = GetComponentsInChildren<Collider2D>();

        // 最初はアクティブ
        ApplyState(true);
    }

    protected override void Die()
    {
        if (_isDown) return;
        _isDown = true;

        // 点滅中を止める（EnemyHealth の _isBlinking を利用）
        StopAllCoroutines();

        // 最後の敵フラグ処理・ドロップ処理・イベント呼び出し
        if (isLastEnemy)
            Siene_Change_Main_Shooting.Instance.lastEnemyDead = true;

        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();
        Siene_Change_Main_Shooting.Instance.UnregisterEnemy(this.gameObject);

        OnDeath?.Invoke();

        // Destroy せずに無力化
        ApplyState(false);
    }

    // ====== 復活 ======
    public void Revive()
    {
        if (!_isDown) return;

        _isDown = false;
        currentHP = maxHP;

        ApplyState(true);

        // 敵リストに再登録
        Siene_Change_Main_Shooting.Instance.RegisterEnemy(this.gameObject);
    }

    // ====== 状態切り替え ======
    private void ApplyState(bool active)
    {
        // Collider の ON/OFF
        if (_colliders != null)
        {
            foreach (var col in _colliders)
            {
                if (col != null)
                    col.enabled = active;
            }
        }

        // 見た目の色変更
        if (_renderers != null)
        {
            if (active)
            {
                // EnemyHealth が保持してる original colors に戻す
                for (int i = 0; i < _renderers.Length; i++)
                {
                    if (_renderers[i] != null)
                        _renderers[i].color = _originalColors[i];
                }
            }
            else
            {
                // 死亡状態 → 薄い色
                foreach (var r in _renderers)
                {
                    if (r != null)
                        r.color = inactiveColor;
                }
            }
        }
    }

    // TokenHealth は HP0 のとき Destroy しないため
    // Update の死亡判定を無効化する
    protected override void Update()
    {
        if (_isDown) return;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }
}
