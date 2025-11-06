using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/LaserStraightShot_WorldPosition")]
public class LaserStraightShot_WorldPosition : AttackPatternSO
{
    [Header("レーザーの演出時間")]
    [SerializeField] private float preLaserDuration = 0.5f;
    [SerializeField] private float laserDuration = 1f;

    [Header("発射方向（角度）")]
    [SerializeField] private int angle = 180; // 0=上, 90=右, 180=下, 270=左

    [Header("発射位置（ワールド座標）")]
    [SerializeField] private Vector3 firePosition = Vector3.zero;
    // 👆 ここに「ワールド座標 (x,y,z)」を直接入力する

    public override async void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, int damage)
    {
        // 🔸 Inspectorに入力されたワールド座標を使用
        Vector3 spawnPos = firePosition;

        // 🔸 Inspectorで指定した角度で撃つ
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        // 🔸 弾生成
        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, spawnPos, rot);
        bullet.transform.position = spawnPos;
        bullet.transform.rotation = rot;

        // 🔸 ダメージ設定
        var bulletDamage = bullet.GetComponentInChildren<BulletDamage>();
        if (bulletDamage != null)
        {
            bulletDamage.damage = damage;
            if (isPlyerBullet)
                bulletDamage.damage *= PlayerStatus.Instance.attackPower;
        }

        // 🔸 レーザー制御
        var laser = bullet.GetComponent<LaserNormal>();
        if (laser == null)
        {
            Debug.LogError($"LaserNormal が {bullet.name} 内に見つかりませんでした。");
            return;
        }

        laser.preLaserTime = preLaserDuration;
        laser.laserTime = laserDuration;

        await laser.LaserSequenceAsync();
    }
}
