using Unity.VisualScripting;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [Header("吸い込み範囲")]
    public float collectRadius = 2.5f;
    [Header("アイテム取得音")]
    public AudioClip collectClip;
    [Header("アイテム回収ライン")]
   public float GetBorder ;
    public GameObject BorderLine;


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
    
}
    private void Awake()
    {

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
}
