using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SideShot")]
public class SideShot : AttackPatternSO
{
    [SerializeField] private float offsetX = 1f;

    public override void Shoot(Enemy enemy)
    {
        Vector3 left = enemy.transform.position + new Vector3(-offsetX, 0, 0);
        Vector3 right = enemy.transform.position + new Vector3(offsetX, 0, 0);

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);

        foreach (var pos in new Vector3[] { left, right })
        {
            GameObject bullet = enemy.GetPool().Get(pos, rot);
            bullet.transform.position = pos;
            bullet.transform.rotation = rot;
        }
    }
}