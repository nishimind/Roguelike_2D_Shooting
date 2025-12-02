using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/円を描きながら下に進む（開始位置オフセット付き）")]
public class Circle_Down_StartOffset : MovePatternSO
{
    [Header("下方向スピード")]
    public float fallSpeed = 2f;

    [Header("円運動の半径")]
    public float radius = 1.2f;

    [Header("円を描く速さ（角速度）")]
    public float angularSpeed = 3f;

    [Header("円運動の開始位置オフセット（敵の位置基準）")]
    public Vector3 startOffset = Vector3.zero;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        if (controller == null) return;

        Rigidbody2D rb = controller._rb;

        float angle = 0f;

        // ★ 初期位置にオフセットを加えた場所を円の中心とする
        Vector3 center = controller.transform.position + startOffset;

        while (controller != null)
        {
            // 角度更新
            angle += angularSpeed * Time.deltaTime;

            // 円運動の位置
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // 中心を下方向に移動
            center += Vector3.down * fallSpeed * Time.deltaTime;

            // 目標位置 = 円運動 + 中心
            Vector3 targetPos = center + new Vector3(x, y, 0);

            rb.MovePosition(targetPos);

            await UniTask.Yield();
        }
    }
}
