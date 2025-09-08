using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Header("敵オブジェクト")]
    private GameObject[] _enemy;
    [SerializeField, Header("敵を生成する時間")]
    private float[] _spawnTimes;

    private float _spawnCount;
    private int _spawnNum;

    // Start is called before the first frame update
    void Start()
    {
        _spawnCount = 0.0f;
        _spawnNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _Spawn();
    }

    //敵グループを生成
    private void _Spawn()
    {
        if (_spawnNum > _enemy.Length - 1) return;
        
            _spawnCount += Time.deltaTime;
            if (_spawnCount >= _spawnTimes[_spawnNum])
            {
            Instantiate(_enemy[_spawnNum]);
                _spawnNum++;
                _spawnCount = 0.0f;
            }
        
    }
}
