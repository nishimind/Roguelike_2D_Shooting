using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SideShot")]
public class SideShot : AttackPatternSO
{
    [SerializeField] private float offsetX = 1f;

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        Vector3 left = position + new Vector3(-offsetX, 0, 0);
        Vector3 right = position + new Vector3(offsetX, 0, 0);

        Quaternion rot = Quaternion.Euler(0, 0, rotation)*Quaternion.FromToRotation(Vector3.up, Vector3.down);

        foreach (var pos in new Vector3[] { left, right })
        {
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, pos, rot);
            // çUåÇóÕÇÉZÉbÉg
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;


            bullet.transform.position = pos;
            bullet.transform.rotation = rot;
        }
    }
}