using System.Collections.Generic;
using UnityEngine;

public class CollisionBase : MonoBehaviour
{
    public HealthBase health;
    private readonly Dictionary<Collider2D, float> nextDamageTime = new();
    public readonly HashSet<Collider2D> stayingColliders = new();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(collision);
        stayingColliders.Add(collision); // Stay対象に追加
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stayingColliders.Remove(collision);
        nextDamageTime.Remove(collision);
    }

    private void FixedUpdate()
    {
        if (stayingColliders.Count == 0)
            return;

        // ここに「あとで消すコライダー」をためておく
        var removeList = new List<Collider2D>();

        // 現在接触中のColliderに対してダメージ間隔チェック
        foreach (var collision in stayingColliders)
        {
            // ① null になってるやつは削除候補に
            if (collision == null)
            {
                removeList.Add(collision);
                continue;
            }

            var bullet = collision.GetComponent<BulletDamage>();
            if (bullet == null) continue;

            // destroyOnHit な弾は継続ダメージ対象じゃないのでスキップ（元の仕様どおり）
            if (bullet.destroyOnHit) continue; // レーザー等のみ継続

            // ② 次のダメージ時間を取得（なければ 0 で初期化）
            if (!nextDamageTime.TryGetValue(collision, out float nextTime))
            {
                nextTime = 0f;
            }

            // ③ ダメージを与えるタイミングなら実行
            if (Time.time >= nextTime)
            {
                TakeDamage(collision);
                nextDamageTime[collision] = Time.time + bullet.damageInterval;
            }
        }

        // ④ foreach が終わったあとに、まとめて削除
        foreach (var col in removeList)
        {
            stayingColliders.Remove(col);
            nextDamageTime.Remove(col);
        }
    }

    protected virtual void TakeDamage(Collider2D collision)
    {
        var bullet = collision.GetComponent<BulletDamage>();
        if (bullet == null) return;

        // ダメージ処理
        if (health != null)
            health.TakeDamage(bullet.damage);

        // 弾の消去
        if (bullet.destroyOnHit)
        {
           
          
               BulletPool.Instance.Release(bullet.originPrefab,collision.gameObject);
          
        }
    }
}
