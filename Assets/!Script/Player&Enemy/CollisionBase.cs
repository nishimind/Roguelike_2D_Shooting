using System.Collections.Generic;
using UnityEngine;

public class CollisionBase : MonoBehaviour
{
    public HealthBase health;
    private readonly Dictionary<Collider2D, float> nextDamageTime = new();
    public readonly HashSet<Collider2D> stayingColliders = new();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(collision);
        stayingColliders.Add(collision); // Stay対象に追加
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stayingColliders.Remove(collision);
        nextDamageTime.Remove(collision);
    }

    private void FixedUpdate()
    {
        var removeList = new List<Collider2D>();

        foreach (var collision in stayingColliders)
        {
            // 無効になったコライダーは後で削除
            if (collision == null)
            {
                removeList.Add(collision);
                continue;
            }

            var bullet = collision.GetComponent<BulletDamage>();
            if (bullet == null) continue;
            if (bullet.destroyOnHit) continue;

            if (!nextDamageTime.ContainsKey(collision))
                nextDamageTime[collision] = 0f;

            if (Time.time >= nextDamageTime[collision])
            {
                TakeDamage(collision);
                nextDamageTime[collision] = Time.time + bullet.damageInterval;
            }
        }

        // ★ まとめて消す
        foreach (var col in removeList)
        {
            stayingColliders.Remove(col);
            nextDamageTime.Remove(col);
        }
    }


    protected virtual void TakeDamage(Collider2D collision)
    {
        var bullet = collision.GetComponent<BulletDamage>();
        if (bullet == null) return;

        // ダメージ処理
        if (health != null)
            health.TakeDamage(bullet.damage);

        // 弾の消去
        if (bullet.destroyOnHit)
        {
           
          
               BulletPool.Instance.Release(bullet.originPrefab,collision.gameObject);
          
        }
    }
}
