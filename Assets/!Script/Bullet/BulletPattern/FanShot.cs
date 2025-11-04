using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/FanShot")]
public class FanShot : AttackPatternSO
{
    //îŒ^‚É’e‚ğL‚°‚ÄŒ‚‚Âƒpƒ^[ƒ“
    [SerializeField] private int bulletCount = 5;       // ’e‚Ì”
    [SerializeField] private float spreadAngle = 45f;   // L‚ª‚éŠp“x

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, int damage)
    {
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // rotation‚ğ’†S‚ÉAZ²‰ñ“]‚ğ’Ç‰Á‚·‚é
            Quaternion rot = Quaternion.Euler(0, 0, rotation) * Quaternion.Euler(0, 0, angle);

            // ’e¶¬
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, rot);

            // ƒ_ƒ[ƒWİ’è
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;

            if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

           // Debug.Log($"FanShot damage: {bulletDamage.damage}  angle: {angle}");
        }
    }
}
