using UnityEngine;

public class BulletSine : BulletBase
{
    [Header("サイン波の設定")]
    public float frequency = 5f;   // 波の周期（大きいほど細かく揺れる）
    public float amplitude = 0.5f; // 揺れの幅

    private float time;
    private Vector3 forwardDir; // 発射時点の進行方向

    protected override void Initialize()
    {
        // 発射時の正面方向を固定
        forwardDir = transform.up;
    }

    protected override void Update()
    {
        // 時間経過
        time += Time.deltaTime * frequency;

        // 前方移動
        Vector3 forward = forwardDir * _speed * Time.deltaTime;

        // サイン波の横方向成分
        Vector3 side = Vector3.Cross(forwardDir, Vector3.forward) * Mathf.Sin(time) * amplitude;

        // 新しい位置を計算
        Vector3 newPosition = transform.position + forward + side;

        // 進行方向を更新して向きを調整
        Vector3 moveDir = newPosition - transform.position;
        if (moveDir.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // 実際に移動
        transform.position = newPosition;
    }
}
