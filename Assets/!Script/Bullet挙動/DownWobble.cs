using UnityEngine;

/// <summary>
/// 「だいたい下方向」に落ちつつ、
/// 横にふんわり＋サイン波でゆらゆら動く弾用の移動スクリプト
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class DownWobbleMover : MonoBehaviour
{
    private Rigidbody2D _rb;

    private float _baseSpeed;
    private float _wobbleAmp;
    private float _wobbleFreq;
    private float _sideBase;

    private float _time;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// ExplodeBullet から初期値をセットしてもらう
    /// </summary>
    public void Init(float speed, float wobbleAmplitude, float wobbleFrequency, float sideBase)
    {
        _baseSpeed = speed;
        _wobbleAmp = wobbleAmplitude;
        _wobbleFreq = wobbleFrequency;
        _sideBase = sideBase;
        _time = 0f;
    }

    private void Update()
    {
        if (_rb == null) return;

        _time += Time.deltaTime;

        // 横方向成分 = ベースドリフト + サイン波ゆらゆら
        float side = _sideBase + Mathf.Sin(_time * _wobbleFreq) * _wobbleAmp;

        // 下方向(-1)ベースで、横に少し寄せた方向
        Vector2 dir = new Vector2(side, -1f).normalized;

        _rb.velocity = dir * _baseSpeed;
    }
}