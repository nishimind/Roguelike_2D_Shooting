using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SpiralRingShot")]
public class SpiralRingShot : AttackPatternSO
{
    [SerializeField] private int bulletCount = 8;
    [SerializeField] private float rotationStep = 10f;
    private float currentRotation = 0f;

    public override void Shoot(Enemy enemy, GameObject bulletPrefab)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = 360f / bulletCount * i + currentRotation;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            GameObject bullet = enemy.GetPool().Get(bulletPrefab, enemy.transform.position, rot);
            bullet.transform.position = enemy.transform.position;
            bullet.transform.rotation = rot;
        }

        currentRotation += rotationStep;
    }
}