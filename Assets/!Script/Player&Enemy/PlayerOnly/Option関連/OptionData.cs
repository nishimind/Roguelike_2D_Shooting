using UnityEngine;

[System.Serializable]
public class OptionData
{
    public  OptionType optionType;       // オプションの種類名
    public GameObject optionPrefab; // プレハブ参照
    public int count;               // 生成数
}
public enum OptionType
{
    Option1,
    Option2,
    Option3
}
