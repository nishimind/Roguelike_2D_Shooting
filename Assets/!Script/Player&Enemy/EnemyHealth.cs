using UnityEngine;

public class EnemyHealth : HealthBase
{
    [Header("Å‘åHP")]
    public int maxHP = 10;

    [Header("Œ»İHP")]
    public float currentHP;
    private EnemyDropper enemyDropper;
    [Header("‚±‚Ì“G‚ª€–S‚µ‚½‚çƒV[ƒ“‘JˆÚ‚©H")]
    public bool isLastEnemy = false;

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
        if (isLastEnemy) 
            Siene_Change_Main_Shooting.Instance.lastEnemyDead = true; 

        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();
      //  Debug.Log("Enemy€–S");
        Destroy(gameObject);
    }
}
