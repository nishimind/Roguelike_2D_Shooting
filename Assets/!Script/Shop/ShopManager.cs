using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("カード設定")]
    [SerializeField] private CardData[] allCards;        // 全カード（重複が入ってもOK）
    [SerializeField] private int cardCount = 3;          // 並べる枚数

    [Header("UI")]
    [SerializeField] private Transform cardParent;       // 並べる場所（Layout Group 推奨）
    [SerializeField] private GameObject cardPrefab;      // カードUIプレハブ
    [SerializeField] private GameObject shopRoot;        // ショップ全体の親（空になったら閉じたい時に）


    [Header("参照")]
    [SerializeField] private PlayerStatus playerStatus;        // 可能ならインスペクターで割り当て

    private readonly List<CardUI> shopCards = new List<CardUI>();
    private int cursorIndex = 0;

    private void Start()
    {
        // playerStatus 未設定なら捜索（なければ警告して以降の操作は止める）
        if (playerStatus == null)
        {
            playerStatus = FindObjectOfType<PlayerStatus>();
            if (playerStatus == null)
            {
                Debug.LogWarning("[ShopManager] PlayerStatus が見つかりません。インスペクターで割り当ててください。");
            }
        }

        SetupShop();
    }

    private void SetupShop()
    {
        // 既存クリア（シーン再入場等で二重生成を避ける）
        shopCards.Clear();
        if (cardParent != null)
        {
            for (int i = cardParent.childCount - 1; i >= 0; i--)
            {
                Destroy(cardParent.GetChild(i).gameObject);
            }
        }

        // null 除去 → 重複排除 → シャッフル → 需要枚数だけ
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
                Debug.LogError("[ShopManager] cardPrefab に CardUI が付いていません。");
                Destroy(obj);
                continue;
            }
            ui.Setup(card);
            shopCards.Add(ui);
        }
    

        cursorIndex = 0;
        UpdateHighlight();
    }

    // --- 入力（Input System） ---
    // アクションの Interactions は「Press（Press Only）」を推奨
    public void Left(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (shopCards.Count == 0) return;

        cursorIndex = Mathf.Max(0, cursorIndex - 1);
        UpdateHighlight();
    }

    public void Right(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (shopCards.Count == 0) return;

        cursorIndex = Mathf.Min(shopCards.Count - 1, cursorIndex + 1);
        UpdateHighlight();
    }

    public void Decide(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        TryPurchase();
    }

    // --- 内部処理 ---
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
            Debug.Log("[Shop] カードがありません。");
            return;
        }
        if (playerStatus == null)
        {
            Debug.LogWarning("[Shop] PlayerStatus が未設定のため購入できません。");
            return;
        }

        var cardUI = shopCards[cursorIndex];
        if (cardUI == null || cardUI.Data == null)
        {
            Debug.LogWarning("[Shop] カード情報が不正です。");
            return;
        }

        var card = cardUI.Data;

        if (playerStatus.Money < card.price)
        {
            Debug.Log("お金が足りません！");
            return;
        }

        // 決済と効果
        playerStatus.Money -= card.price;
        card.ApplyEffect(playerStatus);

        // UIとリスト更新
        Destroy(cardUI.gameObject);
        shopCards.RemoveAt(cursorIndex);

      

        // 残り0なら閉店 or ハイライト更新
        if (shopCards.Count == 0)
        {
            cursorIndex = 0;
            UpdateHighlight();

            if (shopRoot != null)
            {
                shopRoot.SetActive(false); // 閉店したい場合は有効に
            }
            return;
        }

        cursorIndex = Mathf.Clamp(cursorIndex, 0, shopCards.Count - 1);
        UpdateHighlight();
    }
}
