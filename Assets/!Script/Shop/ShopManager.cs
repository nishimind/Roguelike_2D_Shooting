using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private CardData[] allCards;   // 全カード
    [SerializeField] private Transform cardParent; // 並べる場所
    [SerializeField] private GameObject cardPrefab; // カードUIプレハブ
    [SerializeField] private int cardCount = 3;

    private List<CardUI> shopCards = new List<CardUI>();
    private int cursorIndex = 0;
    private PlayerStatus player;

    void Start()
    {
        player = FindObjectOfType<PlayerStatus>();
        SetupShop();
    }

    void SetupShop()
    {
        // ランダム抽選（重複禁止）
        List<CardData> selected = allCards.OrderBy(x => Random.value).Take(cardCount).ToList();

        foreach (var card in selected)
        {
            GameObject obj = Instantiate(cardPrefab, cardParent);
            CardUI ui = obj.GetComponent<CardUI>();
            ui.Setup(card);
            shopCards.Add(ui);
        }
        HighlightCard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cursorIndex = Mathf.Max(0, cursorIndex - 1);
            HighlightCard();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cursorIndex = Mathf.Min(shopCards.Count - 1, cursorIndex + 1);
            HighlightCard();
        }
        else if (Input.GetKeyDown(KeyCode.Return)) // 購入
        {
            TryPurchase();
        }
    }

    void HighlightCard()
    {
        for (int i = 0; i < shopCards.Count; i++)
        {
            shopCards[i].SetHighlight(i == cursorIndex);
        }
    }

    void TryPurchase()
    {
        var cardUI = shopCards[cursorIndex];
        var card = cardUI.Data;

        if (player.Money >= card.price)
        {
            player.Money -= card.price;
            card.ApplyEffect(player);
            Destroy(cardUI.gameObject); // 購入したら消す
            shopCards.RemoveAt(cursorIndex);
            cursorIndex = Mathf.Clamp(cursorIndex, 0, shopCards.Count - 1);
            HighlightCard();
        }
        else
        {
            Debug.Log("お金が足りません！");
        }
    }
}
