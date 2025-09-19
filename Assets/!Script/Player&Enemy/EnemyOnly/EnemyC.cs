
using UnityEngine;

public class EnemyC : Enemy
{
    [SerializeField, Header("この敵専用の初期攻撃パターン")]
    private AttackPatternSO initialPattern;

    protected override void _Initialize()
    {
        if (initialPattern != null)
        {
            // 基底クラスの attackPattern にセット
            SetAttackPattern(initialPattern);
        }
    }
    
    // 例: 特殊な移動を追加
    protected override void _Move()
    {
        // y=2 で止まるなど特殊処理
        if (transform.position.y <= 2f)
        {
            _rb.velocity = Vector2.zero;
            _bAttack = true;
        }
        else
        {
            base._Move();
        }
    }
}