using UnityEngine;
using System.Collections.Generic;

public class EnemyDropper : MonoBehaviour
{
    public enum DropPattern
    {
        RandomOffset, // 少しランダムにずらして配置
        Circle         // 円形に均等配置
    }

    [System.Serializable]
    public class DropItemInfo
    {
        public GameObject prefab; // ドロップするプレハブ
        public int count = 1;     // ドロップする数
    }

    [Header("ドロップするアイテムと数")]
    public List<DropItemInfo> dropItems = new List<DropItemInfo>();

    [Header("ドロップ配置パターン")]
    public DropPattern dropPattern = DropPattern.RandomOffset;

    [Header("ずらし範囲（RandomOffset用）")]
    public float randomRange = 1f;

    [Header("円形配置の半径（Circle用）")]
    public float circleRadius = 1.3f;

    public void DropItems()
    {
        if (dropItems.Count == 0) return;

        foreach (var drop in dropItems)
        {
            if (drop.prefab == null || drop.count <= 0) continue;

            switch (dropPattern)
            {
                case DropPattern.RandomOffset:
                    DropRandomOffset(drop);
                    break;

                case DropPattern.Circle:
                    DropCircle(drop);
                    break;
            }
        }
    }

    // ランダムに少しずらして配置
    private void DropRandomOffset(DropItemInfo drop)
    {
        for (int i = 0; i < drop.count; i++)
        {
            Vector2 offset = new Vector2(
                Random.Range(-randomRange, randomRange),
                Random.Range(-randomRange / 3, randomRange / 3)
            );

            Vector2 spawnPos = (Vector2)transform.position + offset;

            var item = Instantiate(drop.prefab, spawnPos, Quaternion.identity);

            var floatMotion = item.GetComponent<ItemFloatMotion>();
            if (floatMotion != null)
                floatMotion.upOffset = 0.3f;
        }
    }

    // 円形に均等配置
    private void DropCircle(DropItemInfo drop)
    {
        for (int i = 0; i < drop.count; i++)
        {
            float angle = (360f / drop.count) * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 offset = new Vector2(
                Mathf.Cos(rad) * circleRadius,
                Mathf.Sin(rad) * circleRadius
            );

            Vector2 spawnPos = (Vector2)transform.position + offset;

            var item = Instantiate(drop.prefab, spawnPos, Quaternion.identity);

            var floatMotion = item.GetComponent<ItemFloatMotion>();
            if (floatMotion != null)
                floatMotion.upOffset = 0;
        }
    }
}
