using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SpiralShot")]
public class SpiralShot : AttackPatternSO
{
    private float currentAngle = 0f;
    [SerializeField] private float angleStep = 15f;

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        Quaternion rot = Quaternion.Euler(0, 0, currentAngle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, rot);
        // çUåÇóÕÇÉZÉbÉg
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

        bullet.transform.position = position;
        bullet.transform.rotation = rot;

        currentAngle += angleStep;
    }
}
