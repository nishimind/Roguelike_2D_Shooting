using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("敵のHP設定")]
    public int maxHP = 5;
    public int currentHP;

    [SerializeField, Header("死亡時effect")]
    private GameObject deadEffect;

    [Header("ダメージ演出")]
    [SerializeField, Tooltip("点滅時間(秒)")]
    private float damageTime = 0.3f;
    [SerializeField, Tooltip("点滅周期(秒)")]
    private float damageCycle = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float damageTimeCount;
    private bool isDamage;

    private Color originalColor;

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // 元の色を保存
        damageTimeCount = 0;
        isDamage = false;
    }

    void Update()
    {
        DamageBlink();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
        else
        {
            // 点滅開始
            isDamage = true;
            damageTimeCount = 0;
        }
    }

    private void Die()
    {
        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        Destroy(gameObject); // 敵はそのまま消す
    }

    // ダメージ点滅処理（赤点滅）
    private void DamageBlink()
    {
        if (!isDamage) return;

        damageTimeCount += Time.deltaTime;

        float value = Mathf.Repeat(damageTimeCount, damageCycle);

        // 点滅中は赤く → そうでなければ元の色
        spriteRenderer.color = (value < damageCycle * 0.5f) ? Color.red : originalColor;

        if (damageTimeCount >= damageTime)
        {
            damageTimeCount = 0;
            spriteRenderer.color = originalColor; // 元に戻す
            isDamage = false;
        }
    }
}
