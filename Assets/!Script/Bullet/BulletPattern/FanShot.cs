using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/FanShot")]
public class FanShot : AttackPatternSO
{
    //îŒ^‚É’e‚ğL‚°‚ÄŒ‚‚Âƒpƒ^[ƒ“
    [SerializeField] private int bulletCount = 5;       // ’e‚Ì”
    [SerializeField] private float spreadAngle = 45f;   // L‚ª‚éŠp“x

    public override void Shoot(Enemy enemy, GameObject bulletPrefab)
    {
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

            GameObject bullet = enemy.GetPool().Get(bulletPrefab, enemy.transform.position, rot);
            bullet.transform.position = enemy.transform.position;
            bullet.transform.rotation = rot;
        }
    }
}
