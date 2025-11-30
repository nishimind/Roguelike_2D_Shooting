using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/LaserStraightShot_WorldPosition")]
public class LaserStraightShot_WorldPosition : AttackPatternSO
{
    [Header("レーザーの演出時間")]
    [SerializeField] private float preLaserDuration = 0.5f;
    [SerializeField] private float laserDuration = 1f;

    [Header("発射方向（角度）")]
    [SerializeField] private int angle = 180; // 0=上, 90=右, 180=下, 270=左

    [Header("発射位置（ワールド座標・単体）")]
    [SerializeField] private Vector3 firePosition = Vector3.zero;

    [Header("オブジェクト位置を使うか？")]
    [SerializeField] private bool useObjectPositions = false;

    [Header("発射位置に使うオブジェクト名（複数指定可）")]
    [SerializeField] private string[] positionObjectNames;
    // Inspector で "LaserPoint1", "LaserPoint2" みたいに名前を複数入れる

    public override async void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {
        // 🔹 発射位置リストを組み立てる
        var spawnPositions = new System.Collections.Generic.List<Vector3>();

        if (useObjectPositions && positionObjectNames != null && positionObjectNames.Length > 0)
        {
            // オブジェクト名から位置取得（複数）
            foreach (var name in positionObjectNames)
            {
                if (string.IsNullOrWhiteSpace(name)) continue;

                GameObject obj = GameObject.Find(name);
                if (obj != null)
                {
                    spawnPositions.Add(obj.transform.position);
                }
                else
                {
                    Debug.LogWarning(
                        $"[LaserStraightShot_WorldPosition] オブジェクト '{name}' が見つかりませんでした。"
                    );
                }
            }

            // 1個も見つからなかった場合の保険で、単体 firePosition も使っておく
            if (spawnPositions.Count == 0)
            {
                spawnPositions.Add(firePosition);
            }
        }
        else
        {
            // 従来どおり、単体のワールド座標からだけ撃つ
            spawnPositions.Add(firePosition);
        }

        // 🔹 発射角度
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        // 🔹 すべての発射位置からレーザーを生成
        foreach (var spawnPos in spawnPositions)
        {
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, spawnPos, rot);
            bullet.transform.position = spawnPos;
            bullet.transform.rotation = rot;

            // ダメージ設定
            var bulletDamage = bullet.GetComponentInChildren<BulletDamage>();
            if (bulletDamage != null)
            {
                bulletDamage.damage = damage;
                if (isPlyerBullet)
                    bulletDamage.damage *= PlayerStatus.Instance.attackPower;
            }

            // レーザー制御
            var laser = bullet.GetComponent<LaserNormal>();
            if (laser == null)
            {
                Debug.LogError($"LaserNormal が {bullet.name} 内に見つかりませんでした。");
                continue;
            }

            laser.preLaserTime = preLaserDuration;
            laser.laserTime = laserDuration;

            await laser.LaserSequenceAsync();
        }
    }
}
