using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/RingShot")]
public class RingShot : AttackPatternSO
{
    [SerializeField] private int bulletCount = 12;

    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab, int damage)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = 360f / bulletCount * i;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, shootPotision.transform.position, rot);

            // UŒ‚—Í‚ðƒZƒbƒg
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

            bullet.transform.position = shootPotision.transform.position;
            bullet.transform.rotation = rot;
        }
    }
}