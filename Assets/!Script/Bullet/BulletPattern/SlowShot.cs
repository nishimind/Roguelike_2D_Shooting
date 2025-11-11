using UnityEngine;

public class BulletSlowEffect : MonoBehaviour
{
    [Header("減速効果")]
    public float slowAmount = 0.5f;   // 速度を何倍にするか
    public float slowDuration = 2f;   // 効果時間（秒）

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // プレイヤーに当たったかチェック
        if (collision.CompareTag("Player"))
        {
            Debug.Log("タグチェッククリア");
            var playerStatus = PlayerStatus.Instance;
            if (playerStatus != null)
            {
                Debug.Log("nullチェッククリア");
                playerStatus.ApplySlow(slowAmount, slowDuration);
            }

            // 弾を非表示にする（プールに戻す）
            gameObject.SetActive(false);
        }
    }
}