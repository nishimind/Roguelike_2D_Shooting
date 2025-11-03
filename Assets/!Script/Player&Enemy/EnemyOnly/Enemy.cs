using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;
using Cysharp.Threading.Tasks;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class AttackSet
    {
        [Header("攻撃パターン（ScriptableObject）")]
        public AttackPatternSO attackPattern;

        [Header("発射間隔(秒)")]
        public float shootInterval = 1f;
        [Header("使用する弾プレハブ")]
        public GameObject bulletPrefab;

        [Header("最初の発射までの遅延(秒)")]
        public float initialDelay = 0f;

        [HideInInspector] public float shootTimer = 0f;
    }
    public AttackSet attackSet;
    // 攻撃パターンを ScriptableObject で差し替え可能にする


    [SerializeField, Header("弾のプーラー")]
    protected BulletPool _bulletPooler;

    [SerializeField, Header("移動速度")]
    protected float _moveSpeed;

    // プレイヤーを参照する
    protected GameObject _player;

    // コンポーネント
    protected Rigidbody2D _rb;

    // 弾発射のタイマー
    protected float _shootCount;

    // 攻撃してよいか（画面内フラグ）
    protected bool _bAttack;

    // Start は最初に一度だけ呼ばれる
    private async void Start()
    {
        _player = FindAnyObjectByType<PlayerMovement>()?.gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _bAttack = false;

        // BulletPool初期化を待機
        await Cysharp.Threading.Tasks.UniTask.WaitUntil(() => BulletPool.Instance != null);

        _Initialize();
        if (attackSet.bulletPrefab == null)
        {
            Debug.LogError($"{name} の attackSet.bulletPrefab が設定されていません！");
            return;
        }
        BulletPool.Instance.RegisterBulletPrefab(attackSet.bulletPrefab);
    }


    // 派生クラスで初期化を追加できる
    protected virtual void _Initialize() { }

    // Update は毎フレーム呼ばれる
    void Update()
    {
        _Move();
        _Attack();
    }

    // 攻撃の管理
    protected virtual void _Attack()
    {
     

       
            if (attackSet.attackPattern == null || attackSet.bulletPrefab == null) return;

            attackSet.shootTimer += Time.deltaTime;

            if (attackSet.shootTimer >= Mathf.Max(0.05f, attackSet.shootInterval))
            {
            attackSet.attackPattern.Shoot(this, attackSet.bulletPrefab);
                attackSet.shootTimer = 0f;
            }
        }
    
    // 下方向に移動
    protected virtual void _Move()
    {
        _rb.velocity = Vector2.down * _moveSpeed;
    }

    // カメラに映っている間だけ攻撃許可
    /*  private void OnWillRenderObject()
      {
          if (Camera.current != null && Camera.current.CompareTag("MainCamera"))
          {
              if (!_bAttack)
              {
                  _bAttack = true;
                  _shootCount = _shootTime;
                  Debug.Log($"{name} attack enabled!");
              }
          }
      }
    */

    // 確実に動作する方法
    private void OnBecameVisible() { _bAttack = true; }
    private void OnBecameInvisible() { _bAttack = false; }

    // --- 外部から参照するための公開メソッド ---
   
    public GameObject GetPlayer() => _player;

    // ランタイムでパターンを差し替える用
    public void SetAttackPattern(AttackPatternSO pattern)
    {
        attackSet.attackPattern = pattern;
    }
}