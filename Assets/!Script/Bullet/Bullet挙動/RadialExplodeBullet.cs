using UnityEngine;

public class RadialExplodeBullet : ExplodeBullet
{
    [Header("放射ランダム角度")]
    [SerializeField] private float randomAngleOffset = 0f;

    protected override void SpawnDownRain(Vector3 center)
    {
        if (downBulletPrefab == null || downBulletCount <= 0) return;

        float angleStep = 360f / downBulletCount;

        for (int i = 0; i < downBulletCount; i++)
        {
            float angle = angleStep * i;

            // 少しランダムに（0なら完全均等）
            if (randomAngleOffset > 0f)
            {
                angle += Random.Range(-randomAngleOffset, randomAngleOffset);
            }

            float rad = angle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            Quaternion rot = Quaternion.Euler(0, 0, angle - 90f);

            GameObject bullet = BulletPool.Instance.Get(
                downBulletPrefab,
                center,
                rot
            );

            bullet.transform.position = center;
            bullet.transform.rotation = rot;

            // ダメージ
            var dmg = bullet.GetComponent<BulletDamage>();
            if (dmg != null)
            {
                var parentDamage = GetComponent<BulletDamage>();
                if (parentDamage != null)
                {
                    dmg.damage = parentDamage.damage * 0.5f;
                }
            }

            // Rigidbody移動
            var rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir * downBulletSpeed;
            }

            // wobble対応
            var mover = bullet.GetComponent<DownWobbleMover>();
            if (mover != null)
            {
                mover.Init(
                    downBulletSpeed,
                    downWobbleAmplitude,
                    downWobbleFrequency,
                    0f
                );
            }
        }
    }
}