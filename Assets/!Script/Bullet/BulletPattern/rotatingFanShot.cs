using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/RotatingFanShot")]
public class RotatingFanShot : AttackPatternSO
{
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float spreadAngle = 60f;
    [SerializeField] private float rotationStep = 10f;

    private float currentRotation = 0f;

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, int damage)
    {
        float startAngle = -spreadAngle / 2f + currentRotation;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rot = Quaternion.Euler(0, 0, angle)* Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, rot);

            // UŒ‚—Í‚ðƒZƒbƒg
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

            bullet.transform.position = position;
            bullet.transform.rotation = rot;
        }

        currentRotation += rotationStep;
    }
}
