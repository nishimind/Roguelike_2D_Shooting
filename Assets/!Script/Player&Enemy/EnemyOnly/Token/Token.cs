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

    [Header("通常時の色")]
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
        if (_renderers != null && _renderers.Length > 0)
        {
            _originalColors = new Color[_renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                _originalColors[i] = _renderers[i].color;
            }
        }

        _colliders = GetComponentsInChildren<Collider2D>();

        // シーン管理に登録
        Siene_Change_Main_Shooting.Instance.RegisterEnemy(this.gameObject);

        // 最初はアクティブ状態
        ApplyState(true);
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

        // 点滅中なら止める（これをしないと元の色に戻されてしまうことがある）
        if (_isBlinking)
        {
            StopAllCoroutines();
            _isBlinking = false;
        }

        if (isLastEnemy)
            Siene_Change_Main_Shooting.Instance.lastEnemyDead = true;

        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();

        // 敵リストから削除
        Siene_Change_Main_Shooting.Instance.UnregisterEnemy(this.gameObject);

        // Destroy はせずに、薄い色＋当たり判定OFFにする
        ApplyState(false);
    }

    // ==== 復活処理 ====
    public void Revive()
    {
        if (!_isDown) return;

        _isDown = false;
        currentHP = maxHP;

        // アクティブ状態へ戻す
        ApplyState(true);

        // 敵リストに再登録
        Siene_Change_Main_Shooting.Instance.RegisterEnemy(this.gameObject);
    }

    /// <summary>
    /// アクティブ/ダウン状態の切り替え
    /// 色とコライダーをまとめて制御
    /// </summary>
    private void ApplyState(bool active)
    {
        // コライダーON/OFF
        if (_colliders != null)
        {
            foreach (var col in _colliders)
            {
                if (col != null)
                    col.enabled = active;
            }
        }

        // 色変更
        if (_renderers != null)
        {
            if (active)
            {
                // 復活時 or 初期状態 → 元の色 or activeColor に戻す
                for (int i = 0; i < _renderers.Length; i++)
                {
                    if (_renderers[i] == null) continue;

                    if (_originalColors != null && i < _originalColors.Length)
                        _renderers[i].color = _originalColors[i];
                    else
                        _renderers[i].color = activeColor;
                }
            }
            else
            {
                // ダウン時 → 全部 inactiveColor に
                foreach (var r in _renderers)
                {
                    if (r != null)
                        r.color = inactiveColor;
                }
            }
        }
    }

    // ==== 点滅処理 ====
    private void StartBlink()
    {
        if (_renderers == null || _renderers.Length == 0) return;
        if (_isDown) return; // ダウン中は点滅させない

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
            if (_isDown) break; // 死亡してたら途中でやめる

            SetRenderersColor(blinkColor);
            yield return new WaitForSeconds(blinkDuration);

            if (_isDown) break;

            ResetColors();
            yield return new WaitForSeconds(blinkDuration);
        }

        // 死んでなければ最終的に元の色へ
        if (!_isDown)
        {
            ResetColors();
        }

        _isBlinking = false;
    }

    private void SetRenderersColor(Color c)
    {
        if (_renderers == null) return;

        foreach (var r in _renderers)
        {
            if (r != null)
                r.color = c;
        }
    }

    private void ResetColors()
    {
        if (_renderers == null) return;

        // 元の色が保存されていればそれに戻す
        if (_originalColors != null && _originalColors.Length == _renderers.Length)
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] != null)
                    _renderers[i].color = _originalColors[i];
            }
        }
        else
        {
            // 念のためactiveColorで上書き
            foreach (var r in _renderers)
            {
                if (r != null)
                    r.color = activeColor;
            }
        }
    }
}
