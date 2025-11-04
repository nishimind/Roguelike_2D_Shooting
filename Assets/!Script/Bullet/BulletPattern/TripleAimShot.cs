using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/TripleAimShot")]
public class TripleAimShot : AttackPatternSO
{
    [SerializeField] private float sideAngle = 15f;

    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab)
    {
        if (shootPotision== null) return;

        Vector3 dir = PlayerMovement.Instance.transform.position - shootPotision.transform.position;
        Quaternion baseRot = Quaternion.FromToRotation(Vector3.up, dir);

        float[] angles = { -sideAngle, 0, sideAngle };

        foreach (var a in angles)
        {
            Quaternion rot = Quaternion.Euler(0, 0, a) * baseRot;
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab,shootPotision.transform.position, rot);
            bullet.transform.position = shootPotision.transform.position;
            bullet.transform.rotation = rot;
        }
    }
}

