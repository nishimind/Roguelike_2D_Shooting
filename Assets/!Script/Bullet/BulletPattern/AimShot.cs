using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/AimShot")]
public class AimShot : AttackPatternSO
{
    // プレイヤーの方向に弾を撃つパターン
    public override void Shoot(Enemy enemy)
    {
        if (enemy.GetPlayer() == null) return;

        Vector3 dir = enemy.GetPlayer().transform.position - enemy.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);

        GameObject bullet = enemy.GetPool().Get(enemy.transform.position, rot);
        bullet.transform.position = enemy.transform.position;
        bullet.transform.rotation = rot;
    }
}
