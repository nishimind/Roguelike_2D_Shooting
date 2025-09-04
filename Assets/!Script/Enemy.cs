using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //変数を設定
    // [SerializeField,Header("弾オブジェクト")]
    //   private GameObject _bullet;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField, Header("弾の発射する時間")]
    private float _shootTime;
    [SerializeField, Header("弾のプーラー")]
    
    private BulletPool _bulletPooler;
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;

    //プレイヤーを設定する変数
    private GameObject _player;
    private Rigidbody2D _rb;
    //弾を発射する時間をカウントする変数
    private float _shootCount;
    //画面内で攻撃する
    private bool _bAttack;

    // Start is called before the first frame update
    void Start()
    { 
        //プレイヤーを探す プレイヤー消滅後停止
        if(FindAnyObjectByType<PlayerMovement>() )
        {
           _player = FindObjectOfType<PlayerMovement>().gameObject;
        }      
        _shootCount = 0;
        _bAttack = false;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Shooting();
        _Move();
    }

    private void _Shooting()
    {
        //弾を発射する
        _shootCount += Time.deltaTime;
        if(_shootCount < _shootTime) return;

        //弾を生成する
        //  GameObject bulletObj = Instantiate(_bullet);
        GameObject bulletObj = _bulletPooler.Get(transform.position, transform.rotation);

        //生成した弾を敵の座標に設定する
        bulletObj.transform.position = transform.position;
        //敵からプレイヤーに向かって弾を発射する（ベクトル）
        Vector3 dir = _player.transform.position - transform.position;
        bulletObj.transform.rotation = Quaternion.FromToRotation(transform.up,dir);
        _shootCount = 0.0f;
    }

    //下方向に移動
    private void _Move()
    {
        _rb.velocity = Vector2.down * _moveSpeed;
    }

    //カメラに写っている間攻撃する
    private void OnWillRenderObject()
    {
        if(Camera.current.name == "Main Camera")
        {
            _bAttack = true;
        }
    }
}
