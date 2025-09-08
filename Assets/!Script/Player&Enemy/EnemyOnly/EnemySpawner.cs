using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Header("�G�I�u�W�F�N�g")]
    private GameObject[] _enemy;
    [SerializeField, Header("�G�𐶐����鎞��")]
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

    //�G�O���[�v�𐶐�
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
