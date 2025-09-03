using UnityEngine;
using UnityEngine.UI; // Text用
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth; // PlayerHealthスクリプトをアサイン
       
    public TMP_Text hpText;        // TextMeshProを使う場合はこちら

    void Update()
    {
        if (playerHealth != null && hpText != null)
        {
            hpText.text = "HP: " + playerHealth.currentHP + "/" + playerHealth.maxHP;
        }
    }
}
