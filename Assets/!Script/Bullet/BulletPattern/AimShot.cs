using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/AimShot")]
public class AimShot : AttackPatternSO
{
    // プレイヤーの方向に弾を撃つパターン
    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab, int damage)
    {
        if (PlayerMovement.Instance == null) return;

        Vector3 dir =PlayerMovement.Instance.transform.position - shootPotision.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab,shootPotision.transform.position, rot);

        // 攻撃力をセット
        var bulletDamage = bullet.GetComponent<BulletDamage>();
   
        bulletDamage.damage = damage;
        if (isPlyerBullet)  bulletDamage.damage *= PlayerStatus.Instance.attackPower;
      
        bullet.transform.position = shootPotision.transform.position;
        bullet.transform.rotation = rot;
    }
}
