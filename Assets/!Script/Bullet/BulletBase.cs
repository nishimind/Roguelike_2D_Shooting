using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField, Header("’e‚Ì‘¬“x")]
    public float _speed = 5f;

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

