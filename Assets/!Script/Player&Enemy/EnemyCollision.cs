using UnityEngine;

public class EnemyCollision : MonoBehaviour
{public HealthBase health;
    void Start() { health = GetComponent<HealthBase>(); }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bullet = collision.GetComponent<BulletDamage>();
        if (bullet != null)
        {
            // É_ÉÅÅ[ÉWèàóù
           
            if (health != null)
                health.TakeDamage(bullet.damage);

            // íeÇè¡Ç∑
            if (bullet.destroyOnHit)
            {
                var checker = collision.GetComponent<CameraChecker>();
                if (checker != null && checker._pool != null)
                {
                    checker._pool.Release(collision.gameObject);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
