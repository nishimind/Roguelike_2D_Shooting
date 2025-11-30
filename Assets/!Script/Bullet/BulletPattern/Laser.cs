using UnityEngine;
using Cysharp.Threading.Tasks;

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
        var spawnPositions = new System.Collections.Generic.List<Vector3>();

        // 🔹 オブジェクト名から位置を取るパターン
        if (useObjectPositions && positionObjectNames != null && positionObjectNames.Length > 0)
        {
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
                    Debug.LogWarning($"[LaserStraightShot_WorldPosition] オブジェクト '{name}' が見つかりませんでした。");
                }
            }

            // 1つも見つからなかったときの保険
            if (spawnPositions.Count == 0)
            {
                spawnPositions.Add(firePosition);
            }
        }
        else
        {
            // 🔹 固定ワールド座標から撃つパターン
            spawnPositions.Add(firePosition);
        }

        Quaternion rot = Quaternion.Euler(0, 0, angle);

        // 🔹 すべてのレーザーの UniTask をまとめる
        var tasks = new System.Collections.Generic.List<UniTask>();

        foreach (var spawnPos in spawnPositions)
        {
            GameObject bullet = BulletPool.Instance.Get(bulletPrefab, spawnPos, rot);

            // （BulletPool 側で position / rotation をセットしてるなら省略可）
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

            var laser = bullet.GetComponent<LaserNormal>();
            if (laser == null)
            {
                Debug.LogError($"LaserNormal が {bullet.name} 内に見つかりませんでした。");
                continue;
            }

            laser.preLaserTime = preLaserDuration;
            laser.laserTime = laserDuration;

            // レーザー演出の UniTask をリストに追加
            tasks.Add(laser.LaserSequenceAsync());
        }

        // 🔹 全てのレーザー演出が終わるのを待つ（同時再生）
        if (tasks.Count > 0)
        {
            await UniTask.WhenAll(tasks);
        }
    }
}