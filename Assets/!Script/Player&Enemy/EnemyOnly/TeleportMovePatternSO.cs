using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(
    fileName = "TeleportMovePattern",
    menuName = "Enemy/Move Pattern/Teleport"
)]
public class TeleportMovePatternSO : MovePatternSO
{
    public float teleportInterval = 1.5f;

    [Header("画面上部1/3の高さ")]
    public float marginTop = 0.1f;
    public float heightRatio = 1f / 3f;

    [Header("左右のX座標範囲")]
    public float xMin = -5f;
    public float xMax = 5f;

    [Header("次のテレポート位置マーカー")]
    public GameObject previewMarkerPrefab;

    private GameObject currentMarker;
    private Vector2 nextTeleportPos;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        while (controller != null && controller.gameObject.activeInHierarchy)
        {
            nextTeleportPos = CalculateTeleportPosition();
            ShowPreviewMarker(controller);

            await UniTask.Delay(
                (int)(teleportInterval * 1000),
                cancellationToken: controller.GetCancellationTokenOnDestroy()
            );

            Teleport(controller);
        }
    }

    private void Teleport(EnemyMovementController controller)
    {
        if (currentMarker != null)
        {
            GameObject.Destroy(currentMarker);
        }

        controller._rb.position = nextTeleportPos;
    }

    private void ShowPreviewMarker(EnemyMovementController controller)
    {
        if (previewMarkerPrefab == null) return;

        if (currentMarker != null)
        {
            GameObject.Destroy(currentMarker);
        }

        currentMarker = GameObject.Instantiate(
            previewMarkerPrefab,
            nextTeleportPos,
            Quaternion.identity
        );
    }

    private Vector2 CalculateTeleportPosition()
    {
        Camera cam = Camera.main;
        if (cam == null) return Vector2.zero;

        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));

        float yMin = bottomLeft.y + (topRight.y - bottomLeft.y) * (1f - heightRatio);
        float yMax = topRight.y - marginTop;

        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);

        return new Vector2(x, y);
    }
}