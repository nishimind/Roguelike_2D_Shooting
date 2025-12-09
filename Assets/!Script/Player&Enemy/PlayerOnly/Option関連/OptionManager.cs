using UnityEngine;
using System.Collections.Generic;

public class OptionManager : MonoBehaviour
{
    public List<Transform> options = new List<Transform>();

    public float radius = 2f;
    public float rotateSpeed = 100f;
    public bool isGathering = false;

    private float angleOffset = 0f;
    public FormationSO formation;
    void Update()
    {
        if (!isGathering)
        {
            UpdateFormationSO();
        }
        else
        {
            UpdateGather();
        }
    }

    private void UpdateFormationSO()
    {
        int n = options.Count;
        for (int i = 0; i < n; i++)
        {
            float t = (float)i / (n - 1);
            Vector2 pos = formation.GetPosition(t);
            options[i].localPosition = pos;
        }
    }

    // ▼ 集合（自機に寄る）
    private void UpdateGather()
    {
        foreach (var opt in options)
        {
            opt.localPosition = Vector3.MoveTowards(
                opt.localPosition,
                Vector3.zero,
                Time.deltaTime * 3f
            );
        }
    }

    // ▼ ボタンで呼び出し: 集合モード切替
    public void ToggleGather()
    {
        isGathering = !isGathering;
    }

    // ▼ ボタンで呼び出し: 角度変更
    public void RotateFormation(float degrees)
    {
        angleOffset += degrees;
    }
}
