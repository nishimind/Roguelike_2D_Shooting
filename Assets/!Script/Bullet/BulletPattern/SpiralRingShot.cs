using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SpiralRingShot")]
public class SpiralRingShot : AttackPatternSO
{
    [SerializeField] private int bulletCount = 8;
    [SerializeField] private float rotationStep = 10f;
    private float currentRotation = 0f;

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = 360f / bulletCount * i + currentRotation+rotation;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

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