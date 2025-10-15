using UnityEngine;
using DG.Tweening;

public class ItemFloatMotion : MonoBehaviour
{
    [Header("ふわっと浮く高さ")]
    public float floatHeight = 0.5f;

    [Header("ふわっと浮く時間")]
    public float floatUpDuration = 0.3f;

    [Header("落下距離")]
    public float fallDistance = 1.0f;

    [Header("落下時間")]
    public float fallDuration = 1.2f;

    [Header("落下後に止まる高さ調整（0で地面に着地）")]
    public float finalYOffset = 0f;

    private void Start()
    {
        // 最初の位置を保存
        Vector3 startPos = transform.position;

        // シーケンス（順番に動くアニメーション）
        Sequence seq = DOTween.Sequence();

        // ふわっと浮く
        seq.Append(transform.DOMoveY(startPos.y + floatHeight, floatUpDuration)
            .SetEase(Ease.OutQuad));

        // ゆっくり落下
        seq.Append(transform.DOMoveY(startPos.y - fallDistance + finalYOffset, fallDuration)
            .SetEase(Ease.InQuad));
    }
}
