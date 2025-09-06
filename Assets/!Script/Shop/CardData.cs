using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Shop/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public int price;
    public Sprite icon;

    // 効果を実際に適用する関数（例：攻撃力を上げるなど）
    public virtual void ApplyEffect(PlayerStatus player)
    {
        Debug.Log(cardName + " を購入しました！");
    }
}
