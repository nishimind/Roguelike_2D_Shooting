using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    [SerializeField, Header("弾の速度")]
    public float _speed = 5f;

    [SerializeField, Header("回頭速度（どれくらい曲がるか）")]
     float _rotateSpeed = 200f;

    private Rigidbody2D _rb;
    private Transform _target;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // 最も近いEnemyを探す
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float minDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            _target = nearestEnemy.transform;
        }
    }

    void FixedUpdate()
    {
        if (_rb == null) return;

        // 追尾対象がいない場合はそのまま直進
        if (_target == null)
        {
            _rb.velocity = transform.up * _speed;
            return;
        }

        // 敵の方向ベクトルを取得
        Vector2 direction = ((Vector2)_target.position - _rb.position).normalized;

        // 現在の進行方向とターゲット方向の角度差を求める
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        // 少しずつ敵方向へ回頭
        
        
            _rb.angularVelocity = -rotateAmount * _rotateSpeed;
        if (5 >= rotateAmount && rotateAmount >= -5) { _rb.angularVelocity =0; }

        // 前進
        _rb.velocity = transform.up * _speed;
    }
}
