using UnityEngine;

public class BossAttackState : StateMachineBehaviour
{
    [Header("このステートで使う攻撃")]
    public AttackSet attackSet;

    [Header("このステートで使う移動")]
    public MovePatternSO movePattern;

    private BossController boss;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        boss = animator.GetComponent<BossController>();

        if (movePattern != null)
        {
            boss.StartMovePattern(movePattern);
        }

        if (attackSet != null)
        {
            boss.SetCurrentAttack(attackSet);
        }
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        boss.StopMove();
        boss.ClearAttack();
    }
}