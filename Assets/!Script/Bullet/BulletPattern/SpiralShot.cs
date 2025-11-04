using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SpiralShot")]
public class SpiralShot : AttackPatternSO
{
    private float currentAngle = 0f;
    [SerializeField] private float angleStep = 15f;

    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab, int damage)
    {
        Quaternion rot = Quaternion.Euler(0, 0, currentAngle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, shootPotision.transform.position, rot);
        // çUåÇóÕÇÉZÉbÉg
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

        bullet.transform.position = shootPotision.transform.position;
        bullet.transform.rotation = rot;

        currentAngle += angleStep;
    }
}
