using UnityEngine;

public class BulletSlowFast : MonoBehaviour
{
    [SerializeField, Header("最初の速度")]
    private float _initialSpeed = 10f;

    [SerializeField, Header("遅くなる速度")]
    private float _slowSpeed = 3f;

    [SerializeField, Header("遅くなるまでの秒数")]
    private float _changeDelay = 1.5f;

    private Rigidbody2D _rb;
    private bool _isSlowed = false;
    private float _timer = 0f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_rb == null) return;

        _timer += Time.deltaTime;

        // 一定時間経過後に速度を変更
        if (!_isSlowed && _timer >= _changeDelay)
        {
            _isSlowed = true;
        }

        // 現在の速度を適用
        float currentSpeed = _isSlowed ? _slowSpeed : _initialSpeed;
        _rb.velocity = transform.up * currentSpeed;
    }
}
