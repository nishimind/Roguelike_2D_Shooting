using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/プレイヤーを追いかける")]
public class ChasePlayer : MovePatternSO
{
    public float speed = 3f;
    public float homingPower = 5f;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        var rb = controller._rb;
        // プレイヤーが見つかるまで待つ
        await UniTask.WaitUntil(() => controller.Player != null);

        while (controller != null && controller.Player != null)
        {
           Vector2 dir = (controller.Player.position - controller.transform.position).normalized;
        Vector2 v = Vector2.Lerp(controller._rb.velocity, dir * speed, Time.deltaTime * homingPower);
        controller._rb.velocity = v;
            await UniTask.Yield();
        }

        // プレイヤーが消えたら停止
        if (rb != null) rb.velocity = Vector2.zero;
    }
}
