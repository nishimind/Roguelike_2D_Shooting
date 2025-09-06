using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Shop/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public int price;
    public Sprite icon;

    // ���ʂ����ۂɓK�p����֐��i��F�U���͂��グ��Ȃǁj
    public virtual void ApplyEffect(PlayerStatus player)
    {
        Debug.Log(cardName + " ���w�����܂����I");
    }
}
