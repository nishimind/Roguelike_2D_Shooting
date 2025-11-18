using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;  // 敵のプレハブ
    public Vector2 spawnPosition;   // 出現位置
    public float spawnTime;         // 出現タイミング（秒）
}

public class EnemySpawnManager : MonoBehaviour
{
    public List<EnemySpawnData> spawnList; // インスペクタで設定
    private float timer = 0f;
    private int nextIndex = 0;

    void Update()
    {
        timer += Time.deltaTime;

        // spawnListにある敵を順番に出す
        if (nextIndex < spawnList.Count && timer >= spawnList[nextIndex].spawnTime)
        {
            var data = spawnList[nextIndex];
            Instantiate(data.enemyPrefab, data.spawnPosition, Quaternion.identity);
            nextIndex++;
        }
    }

    // 敵が全滅したら次の波を出すような制御も可能
    public void SpawnNextWave(List<EnemySpawnData> waveData)
    {
        spawnList = waveData;
        nextIndex = 0;
        timer = 0f;
    }
}
