using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SpiralRingShot")]
public class SpiralRingShot : AttackPatternSO
{
    [SerializeField] private int bulletCount = 8;
    [SerializeField] private float rotationStep = 10f;
    private float currentRotation = 0f;

    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = 360f / bulletCount * i + currentRotation;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, shootPotision.transform.position, rot);
            bullet.transform.position = shootPotision.transform.position;
            bullet.transform.rotation = rot;
        }

        currentRotation += rotationStep;
    }
}