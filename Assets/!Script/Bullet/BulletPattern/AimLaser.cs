using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/LaserAimShot")]
// --- AimLaser ---
public class AimLaser : AttackPatternSO
{
    public float preLaserDuration = 0.5f;
    public float laserDuration = 1f;

    public override async void Shoot(Vector3 position, int angle, GameObject bulletPrefab, int damage)
    {
        if (PlayerMovement.Instance == null) return;

        Vector3 dir = PlayerMovement.Instance.transform.position - position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, rot);
        bullet.transform.position = position;
        bullet.transform.rotation = rot;

        var bulletDamage = bullet.GetComponentInChildren<BulletDamage>();
        if (bulletDamage != null)
        {
            bulletDamage.damage = damage;
            if (isPlyerBullet)
                bulletDamage.damage *= PlayerStatus.Instance.attackPower;
        }

        var laser = bullet.GetComponent<LaserNormal>();
        if (laser == null)
        {
            Debug.LogError($"LaserNormal が {bullet.name} 内に見つかりませんでした。");
            return;
        }

        laser.preLaserTime = preLaserDuration;
        laser.laserTime = laserDuration;
        await laser.LaserSequenceAsync(); // 完了を待つ（必要に応じて）
    }
}

