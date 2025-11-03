using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/AlternateShot")]
public class AlternateShot : AttackPatternSO
{
    private bool toggle = false;

    public override void Shoot(Enemy enemy, GameObject bulletPrefab)
    {
        float angle = toggle ? -15f : 15f;
        toggle = !toggle;

        Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);
        GameObject bullet = enemy.GetPool().Get(bulletPrefab, enemy.transform.position, rot);
        bullet.transform.position = enemy.transform.position;
        bullet.transform.rotation = rot;
    }
}