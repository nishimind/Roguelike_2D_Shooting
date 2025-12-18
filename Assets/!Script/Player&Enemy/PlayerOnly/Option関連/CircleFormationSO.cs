using UnityEngine;

[CreateAssetMenu(menuName = "Option/円")]
public class CircleFormationSO : FormationSO
{
    public float radius = 2f;
    [Range(0f, 1f)]
    public float slowRate = 0.4f;

    public override Vector2 GetNormalPosition(int index, int count)
    {
        if (count <= 0) return Vector2.zero;

        float angleStep = 360f / count;
        float startAngle;

        if (count % 2 == 1)
        {
            // 奇数個：中央が真上(90度)
            int centerIndex = count / 2;
            startAngle = 90f - angleStep * centerIndex;
        }
        else
        {
            // 偶数個：90度を中心に左右対称
            startAngle = 90f - angleStep * (count / 2 - 0.5f);
        }

        float angle = startAngle + angleStep * index;
        float rad = angle * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
    }

    public override Vector2 GetSlowPosition(int index, int count)
    {
        return GetNormalPosition(index, count) * slowRate;
    }
}