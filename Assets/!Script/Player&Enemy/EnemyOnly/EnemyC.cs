using UnityEngine;

public class EnemyC : Enemy
{
    [SerializeField, Header("この敵専用の初期攻撃パターン")]
    private AttackPatternSO initialPattern;

    [SerializeField, Header("停止するY位置")]
    private float stopPosY = 2f;  // ここで止まる

    protected override void _Initialize()
    {
        base._Initialize(); // 基底クラスの初期化も必ず呼ぶ

        if (initialPattern != null)
        {
            SetAttackPattern(initialPattern);
        }
    }

    protected override void _Move()
    {
        // 指定のY位置で停止
        if (transform.position.y <= stopPosY)
        {
            _rb.velocity = Vector2.zero;  // 移動は止める
            // 攻撃フラグは Enemy 側の OnWillRenderObject() が担当
        }
        else
        {
            base._Move();
        }
    }
}
