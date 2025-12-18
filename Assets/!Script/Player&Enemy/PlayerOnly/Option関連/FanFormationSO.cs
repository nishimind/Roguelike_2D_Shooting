using UnityEngine;

[CreateAssetMenu(menuName = "Option/Formation/Fan")]
public class FanFormationSO : FormationSO
{
    public float radius = 2f;
    public float angleStep = 30f; // 1つずつずらす角度
    [Range(0f, 1f)]
    public float slowRate = 0.4f;

    public override Vector2 GetNormalPosition(int index, int count)
    {
        // 中央基準のインデックス（例: 3個 → -1,0,1）
        float centerIndex = (count - 1) * 0.5f;
        float offset = index - centerIndex;

        // 真上(90度)を基準に左右へ振る
        float angle = 90f - offset * angleStep;
        float rad = angle * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        return dir * radius;
    }

    public override Vector2 GetSlowPosition(int index, int count)
    {
        return GetNormalPosition(index, count) * slowRate;
    }
}
