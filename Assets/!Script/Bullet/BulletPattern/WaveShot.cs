using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/WaveShot")]
public class WaveShot : AttackPatternSO
{
    private float time = 0f;
    [SerializeField] private float waveAmplitude = 30f;
    [SerializeField] private float waveSpeed = 5f;

    public override void Shoot(GameObject shootPotision, GameObject bulletPrefab)
    {
        time += Time.deltaTime * waveSpeed;
        float angle = Mathf.Sin(time) * waveAmplitude;

        Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);
        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, shootPotision.transform.position, rot);
        bullet.transform.position = shootPotision.transform.position;
        bullet.transform.rotation = rot;
    }
}