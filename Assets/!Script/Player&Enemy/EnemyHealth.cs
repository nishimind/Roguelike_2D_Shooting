using UnityEngine;

public class EnemyHealth : HealthBase
{
    [Header("ç≈ëÂHP")]
    public int maxHP = 10;

    [Header("åªç›HP")]
    public float currentHP;
    private EnemyDropper enemyDropper;

    protected override void Start()
    {
        base.Start();
        currentHP = maxHP;
        enemyDropper = GetComponent<EnemyDropper>();
    }

    protected override void Update()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }
    public override void TakeDamage(float damage)
    {
        currentHP -= damage;


      /*  else
        {
            StartBlink();
        }*/
    }
    protected override void Die()
    {
        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();
      //  Debug.Log("EnemyéÄñS");
        Destroy(gameObject);
    }
}
