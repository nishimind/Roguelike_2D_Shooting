using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/StraightDown")]
public class NormalShot: AttackPatternSO
{
    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"NormalShot: 弾プレハブが設定されていません（{shootPotision.name}）");
            return;
        }

        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, shootPotision.transform.position, shootPotision.transform.rotation);
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }
}
