using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/RandomShot")]
public class RandomShot : AttackPatternSO
{
    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab, int damage)
    {
        float angle = Random.Range(-90f, 90f);
        Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, shootPotision.transform.position, rot);

        // çUåÇóÕÇÉZÉbÉg
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

        bullet.transform.position = shootPotision.transform.position;
        bullet.transform.rotation = rot;
    }
}
