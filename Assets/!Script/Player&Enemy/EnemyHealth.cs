using Unity.VisualScripting;
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

        // “GƒXƒ|[ƒ“‚É“o˜^
        Siene_Change_Main_Shooting.Instance.RegisterEnemy(this.gameObject);
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

        // “G€–S‚ÉƒŠƒXƒg‚©‚çíœ
        Siene_Change_Main_Shooting.Instance.UnregisterEnemy(this.gameObject);

        Destroy(gameObject);
    }
}
