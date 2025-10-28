using UnityEngine;

[System.Serializable]
public class OptionData
{
    public string optionName;       // オプションの種類名
    public GameObject optionPrefab; // プレハブ参照
    public int count;               // 生成数
}
