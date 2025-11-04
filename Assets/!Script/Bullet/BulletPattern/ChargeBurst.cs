using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/ChargeBurst")]
public class ChargeBurst : AttackPatternSO
{
    [SerializeField] private int bulletCount = 10;
    [SerializeField] private float spreadAngle = 90f;

    private int charge = 0;

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, int damage)
    {
        charge++;
        if (charge < 3) return; // 3回溜めてから発射

        float angleStep = spreadAngle / (bulletCount - 1);
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = -spreadAngle / 2f + i * angleStep;
            Quaternion rot = Quaternion.Euler(0, 0, angle)* Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, rot);

            // 攻撃力をセット
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

            bullet.transform.position = position;
            bullet.transform.rotation = rot;
        }

        charge = 0; // チャージリセット
    }
}