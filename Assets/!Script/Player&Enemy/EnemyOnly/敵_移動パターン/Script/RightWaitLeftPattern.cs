using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/RightWaitLeft")]
public class RightWaitLeftPattern : MovePatternSO
{
    public float speed = 3f;
    [Header("各動作の時間")]
    public float waitTime = 1f;
    public float moveDistance = 2f;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        var rb = controller._rb;
        var startPos = rb.position;

        // 初期は右方向へ
        int direction = 1;
        var firsttime = true;
        while (true)
        {
            // 基準位置を更新
            startPos = rb.position;

            // ① 指定距離まで移動
            rb.velocity = Vector2.right * direction * speed;
            if (firsttime)
            {
                firsttime = false;
                while (Vector2.Distance(startPos, rb.position) < moveDistance)
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                // 毎フレーム確認して「moveDistance」移動したら止める
                while (Vector2.Distance(startPos, rb.position) < moveDistance * 2)
                {
                    await UniTask.Yield();
                }
            }

            // 到達したので止める
            rb.velocity = Vector2.zero;

            // ② 少し待つ
            await UniTask.Delay((int)(waitTime * 1000));

            // 左右反転
            direction *= -1;
        }
    }
}
