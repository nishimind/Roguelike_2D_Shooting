using UnityEngine;

public class TokenHealth : HealthBase
{
    [Header("最大HP")]
    public int maxHP = 10;

    [Header("現在HP")]
    public float currentHP;

    private EnemyDropper enemyDropper;

    [Header("このトークンが死亡したらシーン遷移フラグを立てる？")]
    public bool isLastEnemy = false;

    [Header("復活時の色（通常時）")]
    [SerializeField] private Color activeColor = Color.white;

    [Header("死亡時の薄い色")]
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);

    [Header("点滅設定")]
    [SerializeField] private Color blinkColor = Color.red;
    [SerializeField] private float blinkDuration = 0.08f;
    [SerializeField] private int blinkCount = 3;

    private SpriteRenderer[] _renderers;
    private Color[] _originalColors;
    private Collider2D[] _colliders;

    private bool _isBlinking = false;
    private bool _isDown = false;

    protected override void Start()
    {
        base.Start();

        currentHP = maxHP;
        enemyDropper = GetComponent<EnemyDropper>();

        _renderers = GetComponentsInChildren<SpriteRenderer>();

        // 元の色を保存
        if (_renderers != null)
        {
            _originalColors = new Color[_renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                _originalColors[i] = _renderers[i].color;
            }
        }

        _colliders = GetComponentsInChildren<Collider2D>();

        // スポーン時に敵として登録
        Siene_Change_Main_Shooting.Instance.RegisterEnemy(this.gameObject);

        ApplyState(true); // アクティブ状態スタート
    }

    protected override void Update()
    {
        if (_isDown) return;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    public override void TakeDamage(float damage)
    {
        if (_isDown) return;

        currentHP -= damage;
        if (currentHP > 0)
        {
            StartBlink();
        }
        else
        {
            currentHP = 0;
            Die();
        }
    }

    protected override void Die()
    {
        if (_isDown) return;
        _isDown = true;

        if (isLastEnemy)
            Siene_Change_Main_Shooting.Instance.lastEnemyDead = true;

        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();

        // 敵リストから削除する（死んでる扱いになる）
        Siene_Change_Main_Shooting.Instance.UnregisterEnemy(this.gameObject);

        // Destroy せずに「薄く」「当たり判定OFF」にする
        ApplyState(false);
    }

    // =========================================================
    //   復活
    // =========================================================
    public void Revive()
    {
        if (!_isDown) return;

        _isDown = false;
        currentHP = maxHP;

        ApplyState(true);

        // 敵リストに再び追加
        Siene_Change_Main_Shooting.Instance.RegisterEnemy(this.gameObject);
    }

    // =========================================================
    //   有効/無効状態の適用（色と当たり判定の変更）
    // =========================================================
    private void ApplyState(bool active)
    {
        // 色変更
        if (_renderers != null)
        {
            Color newColor = active ? activeColor : inactiveColor;

            foreach (var r in _renderers)
                r.color = newColor;
        }

        // 当たり判定
        if (_colliders != null)
        {
            foreach (var col in _colliders)
                col.enabled = active;
        }

        // 色を元に戻す（アクティブ時）
        if (active)
        {
            ResetColors();
        }
    }

    // =========================================================
    //   点滅処理
    // =========================================================
    private void StartBlink()
    {
        if (_renderers == null) return;

        if (_isBlinking)
        {
            StopAllCoroutines();
            ResetColors();
        }

        StartCoroutine(BlinkCoroutine());
    }

    private System.Collections.IEnumerator BlinkCoroutine()
    {
        _isBlinking = true;

        for (int i = 0; i < blinkCount; i++)
        {
            SetRenderersColor(blinkColor);
            yield return new WaitForSeconds(blinkDuration);

            ResetColors();
            yield return new WaitForSeconds(blinkDuration);
        }

        _isBlinking = false;
    }

    private void SetRenderersColor(Color c)
    {
        if (_renderers == null) return;

        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] != null)
                _renderers[i].color = c;
        }
    }

    private void ResetColors()
    {
        if (_renderers == null || _originalColors == null) return;

        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] != null)
                _renderers[i].color = _originalColors[i];
        }
    }
}
