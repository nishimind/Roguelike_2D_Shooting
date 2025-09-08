using UnityEngine;
using UnityEngine.UI; // Text�p
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth; // PlayerHealth�X�N���v�g���A�T�C��
       
    public TMP_Text hpText;        // TextMeshPro���g���ꍇ�͂�����

    void Update()
    {
        if (playerHealth != null && hpText != null)
        {
            hpText.text = "HP: " + playerHealth.currentHP + "/" + playerHealth.maxHP;
        }
    }
}
