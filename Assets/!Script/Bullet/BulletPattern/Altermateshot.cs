using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/AlternateShot")]
public class AlternateShot : AttackPatternSO
{
    private bool toggle = false;

    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab, int damage)
    {
        float angle = toggle ? -15f : 15f;
        toggle = !toggle;

        Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);
        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, shootPotision.transform.position, rot);

        // çUåÇóÕÇÉZÉbÉg
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;


        bullet.transform.position = shootPotision.transform.position;
        bullet.transform.rotation = rot;
    }
}