using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class CardUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;   
    [SerializeField] private TextMeshProUGUI priceText; 
    [SerializeField] private GameObject highlight;

    public CardData Data { get; private set; }

    public void Setup(CardData data)
    {
        Data = data;
        icon.sprite = data.icon;
        nameText.text = data.cardName;
        priceText.text = data.price.ToString();
        SetHighlight(false);
    }

    public void SetHighlight(bool isActive)
    {
        highlight.SetActive(isActive);
    }
}
