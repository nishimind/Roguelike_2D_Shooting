using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/WaveShot")]
public class WaveShot : AttackPatternSO
{
    private float time = 0f;
    [SerializeField] private float waveAmplitude = 30f;
    [SerializeField] private float waveSpeed = 5f;

    public override void Shoot(Enemy enemy, GameObject bulletPrefab)
    {
        time += Time.deltaTime * waveSpeed;
        float angle = Mathf.Sin(time) * waveAmplitude;

        Quaternion rot = Quaternion.Euler(0, 0, angle) * Quaternion.FromToRotation(Vector3.up, Vector3.down);
        GameObject bullet = enemy.GetPool().Get(bulletPrefab, enemy.transform.position, rot);
        bullet.transform.position = enemy.transform.position;
        bullet.transform.rotation = rot;
    }
}