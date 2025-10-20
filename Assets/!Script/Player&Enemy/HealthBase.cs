using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    [Header("最大HP")]
    public int maxHP = 10;

    [Header("現在HP")]
    protected int currentHP;

    [SerializeField, Header("死亡時effect")]
    protected GameObject deadEffect;

    [Header("ダメージ演出")]
    [SerializeField, Header("点滅時間(秒)")]
    private float damageTime = 0.5f;
    [SerializeField, Header("点滅周期(秒)")]
    private float damageCycle = 0.1f;

    protected SpriteRenderer spriteRenderer;
    private float damageTimeCount;
    private bool isDamage;

    protected virtual void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        DamageBlink();
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
        else
        {
            StartBlink();
        }
    }

    protected void StartBlink()
    {
        isDamage = true;
        damageTimeCount = 0;
    }

    // 共通：点滅処理
    private void DamageBlink()
    {
        if (!isDamage) return;

        damageTimeCount += Time.deltaTime;
        float value = Mathf.Repeat(damageTimeCount, damageCycle);
        spriteRenderer.enabled = value >= damageCycle * 0.5f;

        if (damageTimeCount >= damageTime)
        {
            damageTimeCount = 0;
            spriteRenderer.enabled = true;
            isDamage = false;
        }
    }

    // 共通：回復処理
    public virtual void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log($"{gameObject.name} が {amount} 回復。現在HP: {currentHP}");
    }

    // 派生クラスで具体的な死の挙動を定義する
    protected abstract void Die();
}
