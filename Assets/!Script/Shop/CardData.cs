using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Shop/Card")]
public class CardData : ScriptableObject
{
    [Header("表示される名前")]
    public string cardName;
    [Header("表示される説明文")]
    public string description;
    [Header("価格")]
    public int price;
    [Header("アイコン画像")]
    public Sprite icon;
    [Header("カード効果の種類")]
    public CardEffectType effectType;
    [Header("オプションの種類（AddOption用）")]
    public OptionType optionType;
 
    public enum CardEffectType
    {
        AttackUp,
        AddShot,
        Heal,
        Defence,
        Speed,
        ShootTime,
        AddOption

    }
   

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
                player.currentHP = Mathf.Min(player.maxHP, player.currentHP + Mathf.CeilToInt(effectValue));
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

                case CardEffectType.AddOption:  
              AddOptionCount(player.optionTable, optionType, Mathf.CeilToInt(effectValue));
                break;
        }
    }
    public void AddOptionCount(OptionData[] optionTable, OptionType optionName, int amount = 1 )
    {
        if (optionTable == null || optionTable == null)
        {
            Debug.LogWarning("OptionTable が設定されていません。");
            return;
        }

        foreach (var option in optionTable)
        {
            if (option.optionType == optionName)
            {
                option.count += amount;
                Debug.Log($"{optionName} の count を {amount} 増やしました。新しい値: {option.count}");
                return;
            }
        }

        Debug.LogWarning($"{optionName} という名前のオプションが見つかりません。");
    }
}
