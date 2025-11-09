using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/CircularRotatingBurstShot")]
public class BurstShot : AttackPatternSO
{
    [Header("‰~Œ`’e–‹İ’è")]
    public int bulletCount = 12;          // ’e‚Ì”
    public float radius = 1.5f;           // ’e‚Ì”z’u”¼Œa
    public float delayBeforeFire = 2f;    // ”­Ë‚Ü‚Å‚Ì‘Ò‹@ŠÔ
    public float rotationSpeed = 120f;    // ‰ñ“]‘¬“xi“x/•bj

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        // Coroutine‚ÍMonoBehaviour‚Å“®‚©‚·•K—v‚ ‚è
        BulletPatternRunner.Instance.StartCoroutine(FireRoutine(position, bulletPrefab, damage));
    }

    private IEnumerator FireRoutine(Vector3 position, GameObject bulletPrefab, float damage)
    {
        GameObject[] bullets = new GameObject[bulletCount];
        float[] angles = new float[bulletCount];

        // ’e‚ğ‰~Œ`‚É”z’u
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (360f / bulletCount) * i;
            angles[i] = angle;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radius;
            Vector3 spawnPos = position + offset;

            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, spawnPos, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Ã~ó‘Ô

            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet)
                bulletDamage.damage *= PlayerStatus.Instance.attackPower;

            bullets[i] = bullet;
        }

        float elapsed = 0f;

        //  ‰ñ“]’†idelayBeforeFire•bŠÔj
        while (elapsed < delayBeforeFire)
        {
            elapsed += Time.deltaTime;
            float rotateAmount = rotationSpeed * Time.deltaTime;

            // Še’e‚ğ’†S‚ğ²‚É‰ñ“]‚³‚¹‚é
            for (int i = 0; i < bulletCount; i++)
            {
                if (bullets[i] == null) continue;
                angles[i] += rotateAmount;

                Vector3 offset = new Vector3(Mathf.Cos(angles[i] * Mathf.Deg2Rad), Mathf.Sin(angles[i] * Mathf.Deg2Rad), 0) * radius;
                bullets[i].transform.position = position + offset;
            }

            yield return null;
        }

        // ”­ËI
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) yield break;

        Vector3 playerPos = player.transform.position;

        foreach (GameObject bullet in bullets)
        {
            if (bullet == null) continue;
            Vector2 dir = (playerPos - bullet.transform.position).normalized;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = dir * bullet.GetComponent<BulletBase>()._speed;

            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        }
    }
}