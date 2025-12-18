using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
[System.Serializable]
public class OptionData
{
    public OptionType optionType;       // オプションの種類名
    public GameObject optionPrefab; // プレハブ参照
    public int count;               // 生成数
    public List<Transform> generatedOptions = new List<Transform>(); // 生成されたオプションのインスタンスリスト
    public FormationSO formation;
}
public enum OptionType
{
    Option1,
    Option2,
    Option3
}

public class OptionManager : MonoBehaviour
{
    private OptionData[] options = null;

    public float radius = 2f;
    public float rotateSpeed = 100f;
    public bool isGathering = false;
    private bool _prevSlow=false;
    private float angleOffset = 0f;
    
    void Start()
    {
        // OptionData を取得
      options= PlayerStatus.Instance.optionTable;
        // オプションのインスタンスを生成
        foreach (var optData in options)
        {
            for (int i = 0; i < optData.count; i++)
            {
                GameObject optInstance = Instantiate(optData.optionPrefab, transform);
                optData.generatedOptions.Add(optInstance.transform);
            }
        }

        OptionNormalFormation();
    }
  

    void Update()
    {
        bool isSlow = PlayerMovement.Instance.isSlow;

        if (isSlow != _prevSlow)
        {
            if (isSlow)
                OptionSlowFormation();
            else
                OptionNormalFormation();
        }

        _prevSlow = isSlow;
    }
    //通常時の配置　一回だけ呼ぶのでもいいか

    private void OptionNormalFormation()
    {
        foreach (var optData in options)
        {
            int count = optData.generatedOptions.Count;

            for (int i = 0; i < count; i++)
            {
                Vector2 pos =
                    optData.formation.GetNormalPosition(i, count);

                optData.generatedOptions[i]
       .DOLocalMove(pos, 0.2f)
       .SetEase(Ease.OutQuad);

            }
        }
    }


    // 低速時の配置
    private void OptionSlowFormation()
    {
        foreach (var optData in options)
        {
            int count = optData.generatedOptions.Count;

            for (int i = 0; i < count; i++)
            {
                Vector2 pos =
                    optData.formation.GetSlowPosition(i, count);

                optData.generatedOptions[i]
     .DOLocalMove(pos, 0.2f)
     .SetEase(Ease.OutQuad);

            }
        }
    }





}
