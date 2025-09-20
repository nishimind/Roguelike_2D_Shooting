using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 攻撃パターンを ScriptableObject で差し替え可能にする
    [SerializeField, Header("攻撃パターン（ScriptableObject を指定）")]
    protected AttackPatternSO attackPattern;

    [SerializeField, Header("弾の発射間隔(秒)")]
    protected float _shootTime;

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
    void Start()
    {
        // プレイヤーを探す（存在チェック）
        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null)
        {
            _player = pm.gameObject;
        }

        _shootCount = 0f;
        _bAttack = false;
        _rb = GetComponent<Rigidbody2D>();

        _Initialize();
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
        if (!_bAttack || attackPattern == null)
        {
            Debug.LogWarning($"{name} cannot shoot: bAttack={_bAttack}, attackPattern={attackPattern}");
            return;
        }
        

        _shootCount += Time.deltaTime;
        if (_shootCount >= _shootTime)
        {
            Debug.Log($"{name} shooting!");

            attackPattern.Shoot(this);  // ScriptableObject の Shoot 実行
            _shootCount = 0f;
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
    public BulletPool GetPool() => _bulletPooler;
    public GameObject GetPlayer() => _player;

    // ランタイムでパターンを差し替える用
    public void SetAttackPattern(AttackPatternSO pattern)
    {
        attackPattern = pattern;
    }
}