using UnityEngine;

public class EnemyHealth : HealthBase
{
    private EnemyDropper enemyDropper;

    protected override void Start()
    {
        base.Start();
        enemyDropper = GetComponent<EnemyDropper>();
    }

    protected override void Die()
    {
        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        enemyDropper?.DropItems();

        Destroy(gameObject);
    }
}
