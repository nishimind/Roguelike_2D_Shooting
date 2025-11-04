using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/StraightDown")]
public class NormalShot: AttackPatternSO
{
    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, int damage)
    {
      

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, Quaternion.Euler(0, 0, rotation));

        // çUåÇóÕÇÉZÉbÉg
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }
}
