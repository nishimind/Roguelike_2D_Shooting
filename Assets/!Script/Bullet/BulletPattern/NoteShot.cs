using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackPattern/NoteShot")]
public class NoteShot : AttackPatternSO
{ 
[Header("レーン間の発射ズレ(秒)")]
    [SerializeField] private float laneOffset = 0.15f;

[Header("落下開始位置(Y座標)")]
[SerializeField] private float startY = 6f;

[Header("落下するX位置（複数レーン用）")]
[SerializeField] private float[] noteXPositions = { -3f, -1f, 1f, 3f };

public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
{
// 1回のShoot呼び出しで「1セット（全レーン分）」だけ発射する
var helper = new GameObject("NoteShooter").AddComponent<MonoBehaviourHelper>();
helper.StartCoroutine(SpawnOneCycle(bulletPrefab, damage, rotation));
}

private System.Collections.IEnumerator SpawnOneCycle(GameObject bulletPrefab, float damage, int rotation)
{
for (int i = 0; i < noteXPositions.Length; i++)
{
Vector3 spawnPos = new Vector3(noteXPositions[i], startY, 0);

GameObject bullet = BulletPool.Instance.Get(
    bulletPrefab,
    spawnPos,
    Quaternion.Euler(0, 0, rotation)
);

// 弾を下向きに
bullet.transform.rotation =
    Quaternion.Euler(0, 0, rotation) *
    Quaternion.FromToRotation(Vector3.up, Vector3.down);

// ダメージ設定
var bulletDamage = bullet.GetComponent<BulletDamage>();
bulletDamage.damage = damage;
if (isPlyerBullet)
bulletDamage.damage *= PlayerStatus.Instance.attackPower;

// レーンごとに少しズラして発射
yield return new WaitForSeconds(laneOffset);
}


}
}

public class MonoBehaviourHelper : MonoBehaviour { }