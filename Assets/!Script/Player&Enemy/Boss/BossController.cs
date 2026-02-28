using UnityEngine;
public class BossController : MonoBehaviour
{
    public AttackSet CurrentAttack { get; private set; }

    public void SetCurrentAttack(AttackSet set)
    {
        CurrentAttack = set;
    }

    public void ClearAttack()
    {
        CurrentAttack = null;
    }

    public void StartMovePattern(MovePatternSO move)
    {
        move.Execute(this.gameObject.GetComponent<EnemyMovementController>()).Forget();
    }

    public void StopMove()
    {
        // 必要ならキャンセル処理
    }

    // AnimationEventから呼ばれる
    public void Fire()
    {
        if (CurrentAttack == null) return;

        CurrentAttack.attackPattern.Shoot(
            transform.position,
            CurrentAttack.shootAngle,
            CurrentAttack.bulletPrefab,
            CurrentAttack.damage
        );
    }
}