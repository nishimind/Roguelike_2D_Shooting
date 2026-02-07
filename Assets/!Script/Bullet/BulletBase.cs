using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField, Header("弾の速度")]
    public float _speed = 5f;
    [Header("移動角度（度）")]
    [Tooltip("0=右 / 90=上 / -90=下")]
    public float angleDeg = -90f;

    [HideInInspector] public Rigidbody2D _rb;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Initialize();
    }
    protected virtual void Initialize()
    {

    }
    protected virtual void Update()
    {
        if (_rb == null)return;
            // 角度 → 方向ベクトル
        Vector2 direction = Quaternion.Euler(0f, 0f, angleDeg) * Vector2.right;
        _rb.velocity = direction * _speed;
    }
}

