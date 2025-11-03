using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/RandomShot")]
public class RandomShot : AttackPatternSO
{
    public override void Shoot(Enemy enemy, GameObject bulletPrefab)
    {
        float angle = Random.Range(-90f, 90f);
        Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, enemy.transform.position, rot);
        bullet.transform.position = enemy.transform.position;
        bullet.transform.rotation = rot;
    }
}
