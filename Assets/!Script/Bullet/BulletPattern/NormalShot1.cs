using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/StraightDown")]
public class NormalShot: AttackPatternSO
{
    public override void Shoot(Enemy enemy, GameObject bulletPrefab)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"NormalShot: 弾プレハブが設定されていません（{enemy.name}）");
            return;
        }

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, enemy.transform.position, enemy.transform.rotation);
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }
}
