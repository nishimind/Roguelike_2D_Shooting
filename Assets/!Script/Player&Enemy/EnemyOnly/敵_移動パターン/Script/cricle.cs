using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/円を描きながら下に進む（現在位置スタート）")]
public class Circle_Down_CurrentStart : MovePatternSO
{
    [Header("下方向スピード")]
    public float fallSpeed = 2f;

    [Header("円運動の半径")]
    public float radius = 1.2f;

    [Header("円を描く速さ（角速度）")]
    public float angularSpeed = 3f;

    [Header("円運動の中心オフセット")]
    public Vector3 centerOffset = Vector3.zero;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        if (controller == null) return;

        Rigidbody2D rb = controller._rb;

        // ▼ 円の中心
        Vector3 center = controller.transform.position + centerOffset;

        // ▼ 現在位置から角度を逆算（これが重要??）
        Vector3 diff = controller.transform.position - center;
        float angle = Mathf.Atan2(diff.y, diff.x);

        while (controller != null)
        {
            // ▼ 角度更新
            angle += angularSpeed * Time.deltaTime;

            // ▼ 円運動
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // ▼ 中心を下に移動
            center += Vector3.down * fallSpeed * Time.deltaTime;

            // ▼ 目標位置
            Vector3 targetPos = center + new Vector3(x, y, 0);

            rb.MovePosition(targetPos);

            await UniTask.Yield();
        }
    }
}