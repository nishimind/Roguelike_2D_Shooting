using UnityEngine;

public class Bullet_Accele : BulletBase
{
    [Header("‰Á‘¬Ý’è")]
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxSpeed = 10f;
    protected float _currentSpeed;

    protected override void Initialize()
    {
        _currentSpeed = _speed; // BulletBase ‚Ì‰‘¬
    }

    protected override void Update()
    {
        if (_rb == null) return;

        _currentSpeed += acceleration * Time.deltaTime;
        if (maxSpeed >= _speed)
            _currentSpeed = Mathf.Min(_currentSpeed, maxSpeed);
        else
            _currentSpeed = Mathf.Max(_currentSpeed, maxSpeed);
        Vector2 direction =
            Quaternion.Euler(0f, 0f, angleDeg) * Vector2.right;

            _rb.velocity = transform.up * _currentSpeed;
    }
}
