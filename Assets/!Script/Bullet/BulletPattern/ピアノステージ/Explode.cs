ususing UnityEngine;

public class ExplodeBullet : MonoBehaviour
{
    public GameObject explodeBulletPrefab;
    public int bulletCount = 12;
    public float explodeSpeed = 5f;
    public float damage = 1f;

    public void Explode()
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = angleStep * i;

            Quaternion rot = Quaternion.Euler(0, 0, angle);

            GameObject bullet = BulletPool.Instance.Get(
                explodeBulletPrefab,
                transform.position,
                rot
            );

            bullet.transform.position = transform.position;
            bullet.transform.rotation = rot;

            // RigidbodyÇ≈ï˙éÀèÛÇ…îÚÇŒÇ∑
            var rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = rot * Vector2.up;
                rb.velocity = dir * explodeSpeed;
            }

            // É_ÉÅÅ[ÉWê›íË
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            if (bulletDamage != null)
            {
                bulletDamage.damage = damage;
            }
        }
    }
}
