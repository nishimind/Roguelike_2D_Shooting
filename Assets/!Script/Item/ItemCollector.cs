using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ItemCollector : MonoBehaviour
{
   
    private Vector3 normalRadius;
    [Header("アイテム取得音")]
    public AudioClip collectClip;
    [Header("アイテム回収ライン")]
   public float GetBorder ;
    public GameObject BorderLine;



    [Header("低速時吸い込み範囲")]
    public float slowRadiusRate = 1.5f;

    [Header("拡大・縮小にかかる時間")]
    public float scaleDuration = 0.3f;

    private bool isSlow = false;
    private Tweener scaleTween;
    [Range(0f, 1f)]
    public float volume = 0.4f; // うるさくないように控えめ

    public AudioSource audioSource;

    /* private void OnDrawGizmosSelected()
     {
         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(transform.position, collectRadius);
     }*/
    private void Update()
    {
        BorderLine.transform.position = new Vector3(0,GetBorder,0);
        if (gameObject.transform.position.y > GetBorder)
        {
            // シーン上のすべての ItemCollectable を取得
            ItemFloatMotion[] allItems = FindObjectsOfType<ItemFloatMotion>();

            // 関数を順に呼び出す
            foreach (var item in allItems)
            {
                item.Collect(transform);
            }
        }
        // 既存のTweenが動いていたら止める
        scaleTween?.Kill();

        float targetScale = isSlow ? normalRadius.x*slowRadiusRate : normalRadius.x*1;

        // 現在のスケールから滑らかに変化
        scaleTween = transform
            .DOScale(targetScale, scaleDuration)
            .SetEase(Ease.OutQuad);
    }
private void Awake()
    {
        normalRadius=transform.localScale;
        audioSource.spatialBlend = 0f; // 2Dサウンド（画面中央）

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFloatMotion item = other.GetComponent<ItemFloatMotion>();
        if (item != null)
        {
            item.Collect(transform); // プレイヤーに吸い込む処理を呼ぶ
        }
    }
    public void PlaySound()
    {
        if (collectClip != null)
        {
            audioSource.PlayOneShot(collectClip, volume);
        }
    }
    public void OnSlow(InputAction.CallbackContext context)
    {
        isSlow = context.ReadValue<float>() > 0.5f;
    }
}
