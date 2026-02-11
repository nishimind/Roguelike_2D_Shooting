using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading; // 追加

[CreateAssetMenu(menuName = "MovePattern/プレイヤーに突進して止まる")]
public class Charge : MovePatternSO
{
    public float dashSpeed = 8f;
    public float dashTime = 1.0f;
    public float decelerationTime = 1.5f;
    public float homingPower = 2f;

    // 引数に CancellationToken を追加
    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        // 敵のGameObjectが破棄されたら自動で止まるトークンを取得
        var token = controller.GetCancellationTokenOnDestroy();

        try
        {
            // プレイヤーが見つかるまで待つ (トークンを渡す)
            await UniTask.WaitUntil(() => controller.Player != null, cancellationToken: token);

            var rb = controller._rb;

            while (true)
            {
                // ===== 突進フェーズ =====
                float timer = 0f;
                while (timer < dashTime)
                {
                    // プレイヤーが途中でいなくなったらループを抜ける
                    if (controller.Player == null) break;

                    Vector2 dir = (controller.Player.position - controller.transform.position).normalized;
                    Vector2 v = Vector2.Lerp(rb.velocity, dir * dashSpeed, Time.deltaTime * homingPower);
                    rb.velocity = v; // dashSpeedを二重に掛けないよう修正

                    timer += Time.deltaTime;
                    await UniTask.Yield(token); // ここでトークンを渡す
                }

                // ===== 減速フェーズ =====
                timer = 0f;
                Vector2 startVelocity = rb.velocity;

                while (timer < decelerationTime)
                {
                    // 徐々に速度をゼロに近づける (Lerpの第3引数は0~1)
                    rb.velocity = Vector2.Lerp(startVelocity, Vector2.zero, timer / decelerationTime);

                    timer += Time.deltaTime;
                    await UniTask.Yield(token); // ここでトークンを渡す
                }

                // ===== 完全停止 =====
                rb.velocity = Vector2.zero;
                await UniTask.Yield(token);
            }
        }
        catch (System.OperationCanceledException)
        {
            // オブジェクトが破棄された時にここに来るが、何もしなくてOK
            // (エラーログを出さずに安全に終了できる)
        }
    }
}