using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/突進してためる追尾")]
public class DashChasePlayer : MovePatternSO
{
    [Header("突進設定")]
    public float dashSpeed = 10f;
    public float dashTime = 0.4f;

    [Header("減速設定")]
    public float brakeTime = 0.15f;

    [Header("ため設定")]
    public float chargeTime = 0.6f;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        var rb = controller._rb;

        while (controller != null && controller.Player != null)
        {
            /* ========= 突進 ========= */
            float t = 0f;
            while (t < dashTime && controller.Player != null)
            {
                Vector2 dir = (controller.Player.position - controller.transform.position).normalized;
                rb.velocity = dir * dashSpeed;

                t += Time.deltaTime;
                await UniTask.Yield();
            }

            /* ========= 急減速 ========= */
            t = 0f;
            Vector2 startVelocity = rb.velocity;
            while (t < brakeTime)
            {
                rb.velocity = Vector2.Lerp(startVelocity, Vector2.zero, t / brakeTime);
                t += Time.deltaTime;
                await UniTask.Yield();
            }
            rb.velocity = Vector2.zero;

            /* ========= ため ========= */
            await UniTask.Delay(System.TimeSpan.FromSeconds(chargeTime));
        }

        // プレイヤー消失時は完全停止
        if (rb != null)
            rb.velocity = Vector2.zero;
    }
}
