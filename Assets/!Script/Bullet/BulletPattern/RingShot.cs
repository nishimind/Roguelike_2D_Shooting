using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/RingShot")]
public class RingShot : AttackPatternSO
{
    [SerializeField] private int bulletCount = 12;

    public override void Shoot(Enemy enemy, GameObject bulletPrefab)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = 360f / bulletCount * i;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, enemy.transform.position, rot);
            bullet.transform.position = enemy.transform.position;
            bullet.transform.rotation = rot;
        }
    }
}