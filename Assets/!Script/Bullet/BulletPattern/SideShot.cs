using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/SideShot")]
public class SideShot : AttackPatternSO
{
    [SerializeField] private float offsetX = 1f;

    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab)
    {
        Vector3 left = shootPotision.transform.position + new Vector3(-offsetX, 0, 0);
        Vector3 right = shootPotision.transform.position + new Vector3(offsetX, 0, 0);

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.down);

        foreach (var pos in new Vector3[] { left, right })
        {
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, pos, rot);
            bullet.transform.position = pos;
            bullet.transform.rotation = rot;
        }
    }
}