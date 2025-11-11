using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/AimLaserFix")]
// --- AimLaser ---
public class AimLaserFix : AttackPatternSO
{
    public float preLaserDuration = 0.5f;
    public float laserDuration = 1f;
    [Header("発射位置（ワールド座標）")]
    [SerializeField] private Vector3 firePosition = Vector3.zero;
    public override async void Shoot(Vector3 position, int angle, GameObject bulletPrefab, float damage)
    {
        if (PlayerMovement.Instance == null) return;

        
        
        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, firePosition, Quaternion.Euler(0, 0, 0));
        Vector3 dir = PlayerMovement.Instance.transform.position -firePosition;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.transform.position = firePosition;
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

