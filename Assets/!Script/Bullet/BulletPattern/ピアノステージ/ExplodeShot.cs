using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/ExplodeShot")]
public class ExplodeShot : AttackPatternSO
{
    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        // 爆発する弾（ExplodeBullet付きのプレハブ）を1発発射
        Quaternion rot = Quaternion.Euler(0, 0, rotation);

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, rot);
        bullet.transform.position = position;
        bullet.transform.rotation = rot;

        // ダメージ設定（爆発元の弾のダメージ）
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        if (bulletDamage != null)
        {
            bulletDamage.damage = damage;
            if (isPlyerBullet)
                bulletDamage.damage *= PlayerStatus.Instance.attackPower;
        }

        // ExplodeBullet 側で爆発処理するので、ここではこれだけでOK
    }
}
