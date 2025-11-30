using System;
using UnityEngine;
using UnityEngine.Events;   

public class EnemyHealth : HealthBase
{
    [Header("最大HP")]
    public int maxHP = 10;

    [Header("現在HP")]
    public float currentHP;

    private EnemyDropper enemyDropper;

    [Header("この敵が死亡したらシーン遷移か？")]
    public bool isLastEnemy = false;

    [Header("ダメージ時の点滅設定")]
    [SerializeField] private Color blinkColor = Color.red; // 点滅時の色
    [SerializeField] private float blinkDuration = 0.08f;   // 1回の色反転の時間
    [SerializeField] private int blinkCount = 3;            // 何回点滅するか

    private SpriteRenderer[] _renderers;
    private Color[] _originalColors;
    private bool _isBlinking = false;
    private bool _isDead = false;

    public UnityEvent OnDeath;

    protected override void Start()
    {
        base.Start();

        currentHP = maxHP;
        enemyDropper = GetComponent<EnemyDropper>();

        // 見た目キャッシュ（子オブジェクト含め全部）
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        if (_renderers != null && _renderers.Length > 0)
        {
            _originalColors = new Color[_renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                _originalColors[i] = _renderers[i].color;
            }
        }

        // 敵スポーン時に登録
        Siene_Change_Main_Shooting.Instance.RegisterEnemy(this.gameObject);
    }

    protected override void Update()
    {
        // すでに死んでたら何もしない
        if (_isDead) return;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    public override void TakeDamage(float damage)
    {
        if (_isDead) return;

        currentHP -= damage;

        if (currentHP > 0)
        {
            // 生きてるときだけ点滅
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
        if (_isDead) return;   // 二重呼び出し防止
        _isDead = true;

        if (isLastEnemy)
            Siene_Change_Main_Shooting.Instance.lastEnemyDead = true;

        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();

        // 敵死亡時にリストから削除
        Siene_Change_Main_Shooting.Instance.UnregisterEnemy(this.gameObject);
        OnDeath?.Invoke();   // ← ★死亡イベント発火
        Destroy(gameObject);
    }

    // ==========================
    // ここから点滅処理
    // ==========================

    private void StartBlink()
    {
        if (_renderers == null || _renderers.Length == 0) return;

        // すでに点滅中なら一旦止めてリスタート
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
            // 色をblinkColorへ
            SetRenderersColor(blinkColor);
            yield return new WaitForSeconds(blinkDuration);

            // 元の色に戻す
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