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
        Speed,
        ShootTime,

    }
    public CardEffectType effectType;

    public float effectValue; // 攻撃力アップ値や回復量など

    // 効果を実際にプレイヤーに反映する処理
    public void ApplyEffect(PlayerStatus player)
    {
        switch (effectType)
        {
            case CardEffectType.AttackUp:
                Debug.Log("攻撃購入");
                player.attackPower += Mathf.CeilToInt(effectValue);
                break;

            case CardEffectType.AddShot:
              //  player.AddShotType(Mathf.CeilToInt(effectValue)); // 新しいショット解放
                break;

            case CardEffectType.Heal:
                player.health.currentHP = Mathf.Min(player.maxHp, player.health.currentHP + Mathf.CeilToInt(effectValue));
                break;

            case CardEffectType.Defence:
                player.defencePower += Mathf.CeilToInt(effectValue);
                break;

                case CardEffectType.Speed:
                player.speed += Mathf.CeilToInt(effectValue);
                break;

            case CardEffectType.ShootTime:
                player.shootTime -= effectValue;
                break;

        }
    }
}
