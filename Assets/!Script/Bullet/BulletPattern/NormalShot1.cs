using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/StraightDown")]
public class NormalShot: AttackPatternSO
{
    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab, int damage)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"NormalShot: 弾プレハブが設定されていません（{shootPotision.name}）");
            return;
        }

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, shootPotision.transform.position, shootPotision.transform.rotation);

        // 攻撃力をセット
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }
}
