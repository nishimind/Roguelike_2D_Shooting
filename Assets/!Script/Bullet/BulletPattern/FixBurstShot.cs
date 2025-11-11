using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "AttackPattern/FixedBurstShot")]
public class FixBurstShot : AttackPatternSO
{
    [Header("連射設定")]
    public int burstAmount = 3;      // 発射回数
    public float interval = 0.2f;    // 発射間隔（秒）

    public override async void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        // --- ① 最初にプレイヤー方向を取得 ---
        if (PlayerMovement.Instance == null) return;

        Vector3 dir = PlayerMovement.Instance.transform.position - position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);
        // --- ② その方向を固定して連射 ---
        for (int i = 0; i < burstAmount; i++)
        {
            // 弾生成
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, Quaternion.Euler(0, 0, 0));

            // 攻撃力設定
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

            // 弾の向きを進行方向に合わせる
            bullet.transform.rotation = rot;

            // 次弾まで待機（最後の1発後は待たない）
            if (i < burstAmount - 1)
                await Task.Delay((int)(interval * 1000));
        }
    }
}
