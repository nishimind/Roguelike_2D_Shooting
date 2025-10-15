using UnityEngine;
using System.Collections.Generic;

public class EnemyDropper : MonoBehaviour
{
    public enum DropPattern
    {
        RandomOffset, // 少しランダムにずらして配置
        Circle         // 円形に均等配置
    }
    [Header("ドロップするアイテム一覧")]
    public List<GameObject> dropItems = new List<GameObject>();

    [Header("ドロップ数")]
    public int dropCount = 3;

    [Header("ドロップ配置パターン")]
    public DropPattern dropPattern = DropPattern.RandomOffset;

    [Header("ずらし範囲（RandomOffset用）")]
    public float randomRange = 1f;

    [Header("円形配置の半径（Circle用）")]
    public float circleRadius = 2f;

    public void DropItems()
    {
        if (dropItems.Count == 0 || dropCount <= 0)
            return;

        switch (dropPattern)
        {
            case DropPattern.RandomOffset:
                DropRandomOffset();
                break;

            case DropPattern.Circle:
                DropCircle();
                break;
        }
    }

    // ランダムに少しずらして配置
    private void DropRandomOffset()
    {
        for (int i = 0; i < dropCount; i++)
        {
            GameObject itemPrefab = dropItems[Random.Range(0, dropItems.Count)];

            // ランダムなずれ位置を計算
            Vector2 offset = new Vector2(
                Random.Range(-randomRange, randomRange),
                Random.Range(-randomRange/3, randomRange/3)
            );

            Vector2 spawnPos = (Vector2)transform.position + offset;

            Instantiate(itemPrefab, spawnPos, Quaternion.identity);
        }
    }

    // 円形に均等配置
    private void DropCircle()
    {
        for (int i = 0; i < dropCount; i++)
        {
            GameObject itemPrefab = dropItems[Random.Range(0, dropItems.Count)];

            // 円形配置の角度を計算（均等に分布）
            float angle = (360f / dropCount) * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 offset = new Vector2(
                Mathf.Cos(rad) * circleRadius,
                Mathf.Sin(rad) * circleRadius
            );

            Vector2 spawnPos = (Vector2)transform.position + offset;

            Instantiate(itemPrefab, spawnPos, Quaternion.identity);
        }
    }
}