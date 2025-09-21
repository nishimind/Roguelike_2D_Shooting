using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SpiralShot")]
public class SpiralShot : AttackPatternSO
{
    private float currentAngle = 0f;
    [SerializeField] private float angleStep = 15f;

    public override void Shoot(Enemy enemy)
    {
        Quaternion rot = Quaternion.Euler(0, 0, currentAngle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);
        GameObject bullet = enemy.GetPool().Get(enemy.transform.position, rot);
        bullet.transform.position = enemy.transform.position;
        bullet.transform.rotation = rot;

        currentAngle += angleStep;
    }
}
