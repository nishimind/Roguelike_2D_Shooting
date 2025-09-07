using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;



public class ShopManager : MonoBehaviour
{
    [Header("�J�[�h�ݒ�")]
    [SerializeField] private CardData[] allCards;        // �S�J�[�h�i�d���������Ă�OK�j
    [SerializeField] private int cardCount = 3;          // ���ׂ閇��

    [Header("UI")]
    [SerializeField] private Transform cardParent;       // ���ׂ�ꏊ�iLayout Group �����j
    [SerializeField] private GameObject cardPrefab;      // �J�[�hUI�v���n�u
    [SerializeField] private GameObject shopRoot;        // �V���b�v�S�̂̐e�i��ɂȂ�������������Ɂj
    [Header("�m�F�_�C�A���O")]
    [SerializeField] private GameObject confirmPanel; // �m�F�_�C�A���O
    [SerializeField] private TextMeshProUGUI yesText;
    [SerializeField] private TextMeshProUGUI noText;

    private bool isConfirmOpen = false;
    private int confirmIndex = 0; // 0 = Yes, 1 = No

    [Header("�Q��")]
    [SerializeField] private PlayerStatus playerStatus;        // �\�Ȃ�C���X�y�N�^�[�Ŋ��蓖��

    private readonly List<CardUI> shopCards = new List<CardUI>();
    private int cursorIndex = 0;

    private void Start()
    {
        // playerStatus ���ݒ�Ȃ�{���i�Ȃ���Όx�����Ĉȍ~�̑���͎~�߂�j
        if (playerStatus == null)
        {
            playerStatus = FindObjectOfType<PlayerStatus>();
            if (playerStatus == null)
            {
                Debug.LogWarning("[ShopManager] PlayerStatus ��������܂���B�C���X�y�N�^�[�Ŋ��蓖�ĂĂ��������B");
            }
        }

        SetupShop();
    }

    private void SetupShop()
    {
        // �����N���A�i�V�[���ē��ꓙ�œ�d�����������j
        shopCards.Clear();
        if (cardParent != null)
        {
            for (int i = cardParent.childCount - 1; i >= 0; i--)
            {
                Destroy(cardParent.GetChild(i).gameObject);
            }
        }

        // null ���� �� �d���r�� �� �V���b�t�� �� ���v��������
        var pool = allCards
            .Where(c => c != null)
            .Distinct()
            .OrderBy(_ => Random.value)
            .Take(Mathf.Min(cardCount, allCards?.Length ?? 0));

        foreach (var card in pool)
        {
            var obj = Instantiate(cardPrefab, cardParent);
            var ui = obj.GetComponent<CardUI>();
            if (ui == null)
            {
                Debug.LogError("[ShopManager] cardPrefab �� CardUI ���t���Ă��܂���B");
                Destroy(obj);
                continue;
            }
            ui.Setup(card);
            shopCards.Add(ui);
        }
    

        cursorIndex = 0;
        UpdateHighlight();
    }

    // --- ���́iInput System�j ---
    // �A�N�V������ Interactions �́uPress�iPress Only�j�v�𐄏�
    public void Left(InputAction.CallbackContext context)
    {
        if (isConfirmOpen) return; // 
        if (!context.performed) return;
        if (shopCards.Count == 0) return;

        cursorIndex = Mathf.Max(0, cursorIndex - 1);
        UpdateHighlight();
    }

    public void Right(InputAction.CallbackContext context)
    {
        if (isConfirmOpen) return; // 
        if (!context.performed) return;
        if (shopCards.Count == 0) return;

        cursorIndex = Mathf.Min(shopCards.Count - 1, cursorIndex + 1);
        UpdateHighlight();
    }

    public void Decide(InputAction.CallbackContext context)
    {
        if (isConfirmOpen) return; // 
        if (!context.performed) return;
        TryPurchase();
    }

    // --- �������� ---
    private void UpdateHighlight()
    {
        for (int i = 0; i < shopCards.Count; i++)
        {
            var ui = shopCards[i];
            if (ui != null) ui.SetHighlight(i == cursorIndex);
        }
    }

    private void TryPurchase()
    {
        if (shopCards.Count == 0)
        {
            Debug.Log("[Shop] �J�[�h������܂���B");
            return;
        }
        if (playerStatus == null)
        {
            Debug.LogWarning("[Shop] PlayerStatus �����ݒ�̂��ߍw���ł��܂���B");
            return;
        }

        var cardUI = shopCards[cursorIndex];
        if (cardUI == null || cardUI.Data == null)
        {
            Debug.LogWarning("[Shop] �J�[�h��񂪕s���ł��B");
            return;
        }

        var card = cardUI.Data;

        if (playerStatus.Money < card.price)
        {
            Debug.Log("����������܂���I");
            return;
        }

        // ���ςƌ���
        playerStatus.Money -= card.price;
        card.ApplyEffect(playerStatus);

        // UI�ƃ��X�g�X�V
        Destroy(cardUI.gameObject);
        shopCards.RemoveAt(cursorIndex);

      

        // �c��0�Ȃ�X or �n�C���C�g�X�V
        if (shopCards.Count == 0)
        {
            cursorIndex = 0;
            UpdateHighlight();

            if (shopRoot != null)
            {
                shopRoot.SetActive(false); // �X�������ꍇ�͗L����
            }
            return;
        }

        cursorIndex = Mathf.Clamp(cursorIndex, 0, shopCards.Count - 1);
        UpdateHighlight();
    }
    // Cancel�L�[�Ŋm�F�p�l�����J��
    public void Cancel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!isConfirmOpen && confirmPanel != null)
        {
            confirmPanel.SetActive(true);
            isConfirmOpen = true;
            confirmIndex = 0;
            UpdateConfirmHighlight();
        }
    }
    // �m�F���̍��E����
    public void ConfirmLeft(InputAction.CallbackContext context)
    {
        if (!isConfirmOpen || !context.performed) return;

        confirmIndex = Mathf.Max(0, confirmIndex - 1);
        UpdateConfirmHighlight();
    }
    public void ConfirmRight(InputAction.CallbackContext context)
    {
        if (!isConfirmOpen || !context.performed) return;

        confirmIndex = Mathf.Min(1, confirmIndex + 1);
        UpdateConfirmHighlight();
    }
    // �������
    public void ConfirmDecide(InputAction.CallbackContext context)
    {
        if (!isConfirmOpen || !context.performed) return;

        if (confirmIndex == 0)
        {
            // Yes
            //�X�e�[�W�ɂ���ĕς��邱�ƂɂȂ肻��
            SceneManager.LoadScene("Main_Shooting");
        }
        else
        {
            // No
            confirmPanel.SetActive(false);
            isConfirmOpen = false;
        }
    }

    // �n�C���C�g�X�V
    private void UpdateConfirmHighlight()
    {
        if (yesText != null) yesText.color = (confirmIndex == 0 ? Color.yellow : Color.white);
        if (noText != null) noText.color = (confirmIndex == 1 ? Color.yellow : Color.white);
    }

}
