using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "AttackPattern/LaserAimFollowShot")]
public class AimLaserFollow : AttackPatternSO
{
    public float preLaserDuration = 0.5f;
    public float laserDuration = 1f;
    public float rotateSpeed = 360f; // 1秒あたりの回転速度（度）

    public override async void Shoot(Vector3 position, int angle, GameObject bulletPrefab, int damage)
    {
        if (PlayerMovement.Instance == null) return;

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, Quaternion.identity);
        bullet.transform.position = position;

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

       

        // --- 照準完了後にレーザーを発射 ---
      
        laser.laserTime = laserDuration;
        laser.LaserSequenceAsync();
        // --- プレイヤーを追尾して照準 ---
        float elapsed = 0f;
        while (elapsed < preLaserDuration)
        {
            if (PlayerMovement.Instance == null) break;

            Vector3 dir = PlayerMovement.Instance.transform.position - bullet.transform.position;
            Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, dir);

            // スムーズ回転
            bullet.transform.rotation = Quaternion.RotateTowards(
                bullet.transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );

            elapsed += Time.deltaTime;
            await Task.Yield(); // 1フレーム待つ
        }
    }
}
