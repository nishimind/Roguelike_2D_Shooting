using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/斜めに移動して停止")]
public class Diagonal_Move_And_Stop : MovePatternSO
{
    [Header("移動速度")]
    public float speed = 2f;

    [Header("移動方向（角度）")]
    [Tooltip("0=上, 90=右, 180=下, 270=左")]
    public float angle = 225f;   // デフォルト：左下

    [Header("どちらかの条件で停止する")]
    public float moveDistance = 3f;
    public float moveTime = 2f;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        if (controller == null) return;

        Rigidbody2D rb = controller._rb;
        Vector3 startPos = controller.transform.position;

        float timer = 0f;

        // 角度 → 方向ベクトル
        Vector2 moveDir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ).normalized;

        while (controller != null)
        {
            // ▼ 斜め移動
            rb.velocity = moveDir * speed;

            timer += Time.deltaTime;

            float traveled = Vector3.Distance(startPos, controller.transform.position);

            if (traveled >= moveDistance || timer >= moveTime)
                break;

            await UniTask.Yield();
        }

        // ▼ 停止
        if (controller != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
