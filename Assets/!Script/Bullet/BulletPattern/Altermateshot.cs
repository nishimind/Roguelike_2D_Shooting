using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/AlternateShot")]
public class AlternateShot : AttackPatternSO
{
    private bool toggle = false;
    //左右交互　角度どうすればいい？？
    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, int damage)
    {
        float angle = toggle ? -15f : 15f;
        toggle = !toggle;

        Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);
        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, rot);

        // 攻撃力をセット
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;


        bullet.transform.position = position;
        bullet.transform.rotation = rot;
    }
}