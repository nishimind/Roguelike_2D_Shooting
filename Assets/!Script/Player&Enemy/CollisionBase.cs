using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBase : MonoBehaviour
{
    public HealthBase health;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
       TakeDamage(collision);
    }
    protected virtual void TakeDamage(Collider2D collision)
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
