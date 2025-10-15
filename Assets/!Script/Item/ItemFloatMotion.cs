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

    [Header("吸い込みスピード")]
    public float moveSpeed = 6f;

    [Header("回収時の消滅までの遅延")]
    public float destroyDelay = 0.05f;

    [Header("吸い込み開始時のEase効果時間")]
    public float easeInDuration = 0.2f;

    private bool isCollected = false;
    private Transform targetPlayer;
    private Tweener scaleTween;
    private Sequence fallSequence; // ← 追加：アニメーションを保持する変数
    private PlayerStatus status;
    private bool isFinished = false;


    private void Start()
    {
      
            status = GameObject.FindWithTag("PlayerStatus").GetComponent<PlayerStatus>();
     
        // 最初の位置を保存
        Vector3 startPos = transform.position;

        // シーケンスを保持しておく
        fallSequence = DOTween.Sequence();

        // ふわっと浮く
        fallSequence.Append(transform.DOMoveY(startPos.y + floatHeight, floatUpDuration)
            .SetEase(Ease.OutQuad));

        // ゆっくり落下
        fallSequence.Append(transform.DOMoveY(startPos.y - fallDistance + finalYOffset, fallDuration)
            .SetEase(Ease.InQuad));
    }

    private void Update()
    {
        if (!isCollected || targetPlayer == null)
            return;

        // プレイヤーの現在位置に向かって移動
        Vector2 dir = ((Vector2)targetPlayer.position - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

        // 一定距離まで近づいたら吸収完了
        if (Vector2.Distance(transform.position, targetPlayer.position) < 0.2f)
        {
           if(!isFinished) status.Money += 1;
           isFinished = true;
            Destroy(gameObject, destroyDelay);
        }
    }

    public void Collect(Transform player)
    {
        if (isCollected) return;
        isCollected = true;
        targetPlayer = player;

        //  落下アニメーションを中断
        if (fallSequence != null && fallSequence.IsActive())
        {
            fallSequence.Kill(); // これで浮遊・落下アニメーションを止める
        }

        // 衝突をオフにして他のアイテムと干渉しないように
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 吸い込み開始時に軽くエフェクト（少し小さくなるなど）
        scaleTween = transform.DOScale(0.8f, easeInDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }
}
