using UnityEngine;

public class Bullet01 : MonoBehaviour
{
    [SerializeField, Header("’e‚Ì‘¬“x")]
    private float _speed = 5f;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_rb != null)
            _rb.velocity = transform.up * _speed;
    }
}

