using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "AttackPattern/LaserStraightShot_WorldPosition")]
public class LaserStraightShot_WorldPosition : AttackPatternSO
{
    [Header("レーザーの演出時間")]
    [SerializeField] private float preLaserDuration = 0.5f;
    [SerializeField] private float laserDuration = 1f;

    [Header("発射方向（角度）")]
    [SerializeField] private int angle = 180; // 0=上, 90=右, 180=下, 270=左

    // 🔹 ここから “発射位置系の項目はすべて削除”
    // firePosition
    // useObjectPositions
    // positionObjectNames

    public override async void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        // 呼び出し側（EnemyPhaseAttack or Enemy）から渡された position をそのまま使用
        Vector3 spawnPos = position;

        Quaternion rot = Quaternion.Euler(0, 0, angle);

        // 弾生成
        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, spawnPos, rot);
        bullet.transform.position = spawnPos;
        bullet.transform.rotation = rot;

        // ダメージ設定
        var bulletDamage = bullet.GetComponentInChildren<BulletDamage>();
        if (bulletDamage != null)
        {
            bulletDamage.damage = damage;
            if (isPlyerBullet)
                bulletDamage.damage *= PlayerStatus.Instance.attackPower;
        }

        // LaserNormal 実行
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