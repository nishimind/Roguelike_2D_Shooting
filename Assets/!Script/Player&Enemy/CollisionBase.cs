using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBase : MonoBehaviour
{
    public HealthBase health;
    private Dictionary<Collider2D, float> nextDamageTime = new();


    private void OnTriggerEnter2D(Collider2D collision)
    {
       TakeDamage(collision);
    }
    protected virtual void TakeDamage(Collider2D collision)
    {
        var bullet = collision.GetComponent<BulletDamage>();
        if (bullet != null)
        {
            // ダメージ処理

            if (health != null)
                health.TakeDamage(bullet.damage);

            // 弾を消す
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        var bullet = collision.GetComponent<BulletDamage>();
        if (bullet == null) return;

        // destroyOnHit=false（つまりレーザーなど）の場合だけ継続処理
        if (bullet.destroyOnHit) return;

        if (!nextDamageTime.ContainsKey(collision))
            nextDamageTime[collision] = 0f;

        if (Time.time >= nextDamageTime[collision])
        {
            TakeDamage(collision);
            nextDamageTime[collision] = Time.time + bullet.damageInterval;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nextDamageTime.ContainsKey(collision))
            nextDamageTime.Remove(collision);
    }
}
