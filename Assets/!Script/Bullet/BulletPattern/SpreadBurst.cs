using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SpreadBurst")]
public class SpreadBurst : AttackPatternSO
{
    [SerializeField] private int bulletsPerBurst = 3;
    [SerializeField] private float spreadAngle = 20f;

    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab)
    {
        for (int i = 0; i < bulletsPerBurst; i++)
        {
            float offset = (i - (bulletsPerBurst - 1) / 2f) * spreadAngle;
            Quaternion rot = Quaternion.Euler(0, 0, offset) * Quaternion.FromToRotation(Vector3.up, Vector3.down);

            GameObject bullet = BulletPool.Instance.Get(bulletPrefab,shootPotision.transform.position, rot);
            bullet.transform.position = shootPotision.transform.position;
            bullet.transform.rotation = rot;
        }
    }
}
