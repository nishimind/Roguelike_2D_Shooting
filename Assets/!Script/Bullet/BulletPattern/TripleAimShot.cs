using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/TripleAimShot")]
public class TripleAimShot : AttackPatternSO
{
    [SerializeField] private float sideAngle = 15f;

    public override void Shoot(Enemy enemy)
    {
        if (enemy.GetPlayer() == null) return;

        Vector3 dir = enemy.GetPlayer().transform.position - enemy.transform.position;
        Quaternion baseRot = Quaternion.FromToRotation(Vector3.up, dir);

        float[] angles = { -sideAngle, 0, sideAngle };

        foreach (var a in angles)
        {
            Quaternion rot = Quaternion.Euler(0, 0, a) * baseRot;
            GameObject bullet = enemy.GetPool().Get(enemy.transform.position, rot);
            bullet.transform.position = enemy.transform.position;
            bullet.transform.rotation = rot;
        }
    }
}

