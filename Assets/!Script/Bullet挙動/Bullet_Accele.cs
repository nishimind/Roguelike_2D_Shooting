using UnityEngine;

public class Bullet_Accele : BulletBase
{
    [Header("‰Á‘¬İ’è")]
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxSpeed = 10f;

    [Header("‹È‚°İ’è")]
    [SerializeField] private float angularSpeed = 0f; // ‹È‚°‚éŒü‚«‚Æ‹­‚³

    protected float _currentSpeed;
    private float _currentAngle;

    protected override void Initialize()
    {
        _currentSpeed = _speed;
        _currentAngle = angleDeg; // ‰ŠúŠp“x‚ğ•Û
    }

    protected override void Update()
    {
        if (_rb == null) return;

        // ‰Á‘¬
        _currentSpeed += acceleration * Time.deltaTime;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxSpeed);

        // Œü‚«‚ğ‹È‚°‚é
        _currentAngle += angularSpeed * Time.deltaTime;

        Vector2 dir = Quaternion.Euler(0f, 0f, _currentAngle) * Vector2.right;
        _rb.velocity = dir * _currentSpeed;
    }
}
