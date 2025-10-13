using UnityEngine;
using System.Collections.Generic;

public class EnemyDropper : MonoBehaviour
{
    public enum DropPattern
    {
        Up,        // 真上に飛ぶ
        AllAround, // 全方位
        Random     // ランダム方向
    }
    [Header("ドロップするアイテム一覧")]
    public List<GameObject> dropItems = new List<GameObject>();

    [Header("ドロップ数")]
    public int dropCount = 3;

    [Header("飛び散り方")]
    public DropPattern dropPattern = DropPattern.Up;

    [Header("飛び散る強さ")]
    public float dropForce = 3f;

    [Header("上方向に飛ぶ角度（DropPattern.Up用）")]
    public float spreadAngle = 15f;

    public void DropItems()
    {
        if (dropItems.Count == 0) return;

        for (int i = 0; i < dropCount; i++)
        {
            // ランダムにアイテム選択
            GameObject itemPrefab = dropItems[Random.Range(0, dropItems.Count)];
            GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);

            // Rigidbody2D を取得
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            // 飛び散る方向を計算
            Vector2 dir = Vector2.up; // デフォルト

            switch (dropPattern)
            {
                case DropPattern.Up:
                    float angle = Random.Range(-spreadAngle, spreadAngle);
                    dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
                    break;

                case DropPattern.AllAround:
                    float randomAngle = Random.Range(0, 360f);
                    dir = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
                    break;

                case DropPattern.Random:
                    dir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.2f, 1f)).normalized;
                    break;
            }

            // 力を加える
            rb.AddForce(dir * dropForce, ForceMode2D.Impulse);
        }
    }
}
