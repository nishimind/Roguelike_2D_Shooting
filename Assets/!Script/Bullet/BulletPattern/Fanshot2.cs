using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/FanShot2")]
public class FanShot2 : AttackPatternSO
{
    // 扇型に弾を広げて撃つパターン
    [SerializeField] private int bulletCount = 5;       // 弾の数
    [SerializeField] private float spreadAngle = 45f;   // 広がる角度
    [SerializeField] private float offset = 1f;         // 弾の発射位置のオフセット（左右に配置する距離）

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            // 弾の角度を決める
            float angle = startAngle + angleStep * i;
            Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.Euler(0, 0, rotation);

            // 発射位置を敵の両サイドに設定
            Vector3 sideOffset = new Vector3(i % 2 == 0 ? -offset : offset, 0, 0); // iが偶数なら左、奇数なら右
            Vector3 spawnPosition = position + sideOffset;

            // 弾の生成
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, spawnPosition, rot);

            // 攻撃力をセット
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

            bullet.transform.position = spawnPosition;
            bullet.transform.rotation = rot;
        }
    }
}