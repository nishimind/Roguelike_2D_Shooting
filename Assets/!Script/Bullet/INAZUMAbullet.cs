using UnityEngine;

public class INAZUMABullet : BulletBase
{
    [SerializeField, Header("最初の速度")]
    private float _initialSpeed = 10f;

    [SerializeField, Header("遅くなる速度")]
    private float _slowSpeed = 3f;

    [SerializeField, Header("遅くなるまでの秒数")]
    private float _changeDelay = 1.5f;

    [SerializeField, Header("停止するまでの秒数（遅くなってから）")]
    private float _stopDelay = 1.5f;

    [SerializeField, Header("停止後に再発射するまでの秒数")]
    private float _restartDelay = 2f;

    [SerializeField, Header("再発射時の速度")]
    private float _relaunchSpeed = 12f;

    private bool _isSlowed = false;
    private bool _isStopped = false;
    private bool _isRelaunched = false;

    private float _timer = 0f;
    private Transform _player;

    protected void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void Update()
    {
        if (_rb == null) return;

        _timer += Time.deltaTime;

        // 段階的な状態遷移
        if (!_isSlowed && _timer >= _changeDelay)
        {
            _isSlowed = true;
        }
        else if (_isSlowed && !_isStopped && _timer >= _changeDelay + _stopDelay)
        {
            _isStopped = true;
        }
        else if (_isStopped && !_isRelaunched && _timer >= _changeDelay + _stopDelay + _restartDelay)
        {
            _isRelaunched = true;
            RelaunchTowardsPlayer();
        }

        // 各状態での動作
        if (_isRelaunched)
        {
            // 再発射中は Rigidbody の速度を維持
            return;
        }
        else if (_isStopped)
        {
            _rb.velocity = Vector2.zero;
        }
        else
        {
            float currentSpeed = _isSlowed ? _slowSpeed : _initialSpeed;
            _rb.velocity = transform.up * currentSpeed;
        }
    }

    private void RelaunchTowardsPlayer()
    {
        if (_player == null) return;

        // プレイヤーの方向を向く
        Vector2 direction = (_player.position - transform.position).normalized;
        transform.up = direction;

        // 新しい速度で再発射
        _rb.velocity = direction * _relaunchSpeed;
    }
}