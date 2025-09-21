using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/StraightDown")]
public class NormalShot: AttackPatternSO
{
    public override void Shoot(Enemy enemy)
    {
        GameObject bullet = enemy.GetPool().Get(enemy.transform.position, enemy.transform.rotation);
        bullet.transform.position = enemy.transform.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }
}
