using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Shop/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public int price;
    public Sprite icon;

    public enum CardEffectType
    {
        AttackUp,
        AddShot,
        Heal,
        Defence,
        Speed
    }
    public CardEffectType effectType;

    public int effectValue; // �U���̓A�b�v�l��񕜗ʂȂ�

    // ���ʂ����ۂɃv���C���[�ɔ��f���鏈��
    public void ApplyEffect(PlayerStatus player)
    {
        switch (effectType)
        {
            case CardEffectType.AttackUp:
                Debug.Log("�U���w��");
                player.attackPower += effectValue;
                break;

            case CardEffectType.AddShot:
                player.AddShotType(effectValue); // �V�����V���b�g���
                break;

            case CardEffectType.Heal:
                player.health.currentHP = Mathf.Min(player.maxHp, player.health.currentHP + effectValue);
                break;
        }
    }
}
