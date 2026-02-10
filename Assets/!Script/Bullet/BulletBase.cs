using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField, Header("弾の速度")]
    public float _speed = 5f;
    [Header("移動角度（度）")]
    [Tooltip("0=右 / 90=上 / -90=下")]
    [HideInInspector] public float angleDeg = -90f;

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
      
        if (_rb != null)
            _rb.velocity = transform.up * _speed;
    }
}

