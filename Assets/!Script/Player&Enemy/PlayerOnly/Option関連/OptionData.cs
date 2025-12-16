using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    public  OptionType optionType;       // オプションの種類名
    public GameObject optionPrefab; // プレハブ参照
    public int count;               // 生成数
    public List<Transform> generatedOptions = new List<Transform>(); // 生成されたオプションのインスタンスリスト
}
public enum OptionType
{
    Option1,
    Option2,
    Option3
}
