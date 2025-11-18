using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : MonoBehaviour
{
    public void DestroyAllEnemies()
    {
        // "Enemy" タグが付いた全ての敵を取得
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 全ての敵を破壊
        foreach (GameObject enemy in enemies)
        {
           enemy.GetComponent<EnemyHealth>()?.TakeDamage(9999); // 敵の体力スクリプトに大ダメージを与える
        }
    }
}
