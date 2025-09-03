using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //変数を設定
    [SerializeField,Header("弾オブジェクト")]
    private GameObject _bullet;
    [SerializeField, Header("弾の発射する時間")]
    private float _shootTime;

    //プレイヤーを設定する変数
    private GameObject _player;
    //弾を発射する時間をカウントする変数
    private float _shootCount;


    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーを探す
        _player = FindObjectOfType<PlayerMovement>().gameObject;
        _shootCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _Shooting();
    }

    private void _Shooting()
    {
        //弾を発射する
        _shootCount += Time.deltaTime;
        if(_shootCount < _shootTime) return;

        //弾を生成する
        GameObject bulletObj = Instantiate(_bullet);
        //生成した弾を敵の座標に設定する
        bulletObj.transform.position = transform.position;
        //敵からプレイヤーに向かって弾を発射する（ベクトル）
        Vector3 dir = _player.transform.position - transform.position;
        bulletObj.transform.rotation = Quaternion.FromToRotation(transform.up,dir);
        _shootCount = 0.0f;
    }
}
