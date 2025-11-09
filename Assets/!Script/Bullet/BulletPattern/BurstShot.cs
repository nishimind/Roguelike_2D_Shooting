using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "AttackPattern/CircularRotatingBurstShot")]
public class BurstShot : AttackPatternSO
{
    [Header("円形弾幕設定")]
    public int bulletCount = 12;          // 弾の数
    public float radius = 1.5f;           // 弾の配置半径
    public float delayBeforeFire = 2f;    // 発射までの待機時間
    public float rotationSpeed = 120f;    // 回転速度（度/秒）

    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        // 発射時の呼び出し側が敵なので、敵Transformを渡す
        // positionだけだと後で追従できない
        GameObject enemy = GameObject.FindWithTag("Enemy");
        if (enemy != null)
        {
            FireAsync(enemy.transform, bulletPrefab, damage).Forget();
        }
        else
        {
            // フォールバック（敵が見つからない場合）
            FireAsync(null, bulletPrefab, damage).Forget();
        }
    }

    private async UniTaskVoid FireAsync(Transform enemy, GameObject bulletPrefab, float damage)
    {
        GameObject[] bullets = new GameObject[bulletCount];
        float[] angles = new float[bulletCount];

        Vector3 centerPos = enemy != null ? enemy.position : Vector3.zero;

        // 弾を円形に配置
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (360f / bulletCount) * i;
            angles[i] = angle;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radius;
            Vector3 spawnPos = centerPos + offset;

            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, spawnPos, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            var bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.damage = damage;
            if (isPlyerBullet)
                bulletDamage.damage *= PlayerStatus.Instance.attackPower;

            bullets[i] = bullet;
        }

        float elapsed = 0f;

        // 🌀 回転＆追従中（delayBeforeFire秒間）
        while (elapsed < delayBeforeFire)
        {
            elapsed += Time.deltaTime;
            float rotateAmount = rotationSpeed * Time.deltaTime;

            // 中心を追従（敵が生きている場合）
            if (enemy != null)
                centerPos = enemy.position;

            for (int i = 0; i < bulletCount; i++)
            {
                if (bullets[i] == null) continue;
                angles[i] += rotateAmount;

                Vector3 offset = new Vector3(Mathf.Cos(angles[i] * Mathf.Deg2Rad), Mathf.Sin(angles[i] * Mathf.Deg2Rad), 0) * radius;
                bullets[i].transform.position = centerPos + offset;
            }

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        // 🎯 一斉発射
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

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