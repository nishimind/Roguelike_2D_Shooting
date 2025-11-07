using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/TripleAimShot")]
public class TripleAimShot : AttackPatternSO
{
    [SerializeField] private float sideAngle = 15f;

    public override void Shoot(Vector3 position, int angle, GameObject bulletPrefab, float damage)
    {
        if (position== null) return;

        Vector3 dir = PlayerMovement.Instance.transform.position - position;
        Quaternion baseRot = Quaternion.FromToRotation(Vector3.up, dir);

        float[] angles = { -sideAngle, 0, sideAngle };

        foreach (var a in angles)
        {
            Quaternion rot = Quaternion.Euler(0, 0, angle)*   Quaternion.Euler(0, 0, a) * baseRot;
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab,position, rot);
            // çUåÇóÕÇÉZÉbÉg
            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

            bullet.transform.position = position;
            bullet.transform.rotation = rot;
        }
    }
}

