using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/プレイヤーに突進して止まる")]
public class Charge : MovePatternSO
{
    public float dashSpeed = 8f;        // 突進速度
    public float dashTime = 1.0f;        // 突進時間
    public float decelerationTime = 1.5f; // 減速にかかる時間

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        if (controller == null || controller.Player == null) return;

        var rb = controller._rb;

        // ===== 突進フェーズ =====
        float timer = 0f;
        while (timer < dashTime && controller.Player != null)
        {
            Vector2 dir = (controller.Player.position - controller.transform.position).normalized;
            rb.velocity = dir * dashSpeed;

            timer += Time.deltaTime;
            await UniTask.Yield();
        }

        // ===== 減速フェーズ =====
        Vector2 startVelocity = rb.velocity;
        timer = 0f;

        while (timer < decelerationTime)
        {
            rb.velocity = Vector2.Lerp(startVelocity, Vector2.zero, timer / decelerationTime);

            timer += Time.deltaTime;
            await UniTask.Yield();
        }

        // ===== 完全停止 =====
        rb.velocity = Vector2.zero;
    }
}
