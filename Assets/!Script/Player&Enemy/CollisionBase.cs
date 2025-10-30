using System.Collections.Generic;
using UnityEngine;

public class CollisionBase : MonoBehaviour
{
    public HealthBase health;
    private readonly Dictionary<Collider2D, float> nextDamageTime = new();
    private readonly HashSet<Collider2D> stayingColliders = new();

    private void OnTriggerEnter2D(Collider2D collision)
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
        // 現在接触中のColliderに対してダメージ間隔チェック
        foreach (var collision in stayingColliders)
        {
            var bullet = collision.GetComponent<BulletDamage>();
            if (bullet == null) continue;
            if (bullet.destroyOnHit) continue; // レーザー等のみ継続

            if (!nextDamageTime.ContainsKey(collision))
                nextDamageTime[collision] = 0f;

            if (Time.time >= nextDamageTime[collision])
            {
                TakeDamage(collision);
                nextDamageTime[collision] = Time.time + bullet.damageInterval;
            }
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
            var checker = collision.GetComponent<CameraChecker>();
            if (checker != null && checker._pool != null)
                checker._pool.Release(collision.gameObject);
            else
                Destroy(collision.gameObject);
        }
    }
}
