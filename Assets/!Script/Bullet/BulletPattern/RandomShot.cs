using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/RandomShot")]
public class RandomShot : AttackPatternSO
{
    public override void Shoot(Vector3 position, int angle, GameObject bulletPrefab, float damage)
    {
        float rotation = Random.Range(-90f, 90f);
        Quaternion rot = Quaternion.Euler(0, 0, rotation) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, rot);

        // çUåÇóÕÇÉZÉbÉg
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

        bullet.transform.position = position;
        bullet.transform.rotation = rot;
    }
}
