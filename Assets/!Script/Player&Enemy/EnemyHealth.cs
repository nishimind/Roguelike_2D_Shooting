using UnityEngine;

public class EnemyHealth : HealthBase
{
    [Header("ç≈ëÂHP")]
    public int maxHP = 10;

    [Header("åªç›HP")]
    public int currentHP;
    private EnemyDropper enemyDropper;

    protected override void Start()
    {
        base.Start();
        currentHP = maxHP;
        enemyDropper = GetComponent<EnemyDropper>();
    }
   protected override void TakeDamage(int damage)
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
    protected override void Die()
    {
        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();

        Destroy(gameObject);
    }
}
