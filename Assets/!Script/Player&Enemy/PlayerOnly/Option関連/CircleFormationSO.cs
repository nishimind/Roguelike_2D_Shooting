using UnityEngine;

[CreateAssetMenu(menuName = "Option/Formation/Circle")]
public class CircleFormationSO : FormationSO
{
    [Header("円形フォーメーション設定")]
    public float radius = 2f;         // 半径
    public float startAngle = 0f;     // 開始角度（度数）
    public bool clockwise = false;    // 回転方向

    /// <summary>
    /// index と count をもとに円形フォーメーションの座標を返す
    /// </summary>
    protected override Vector2 GetPosition(int index, int count)
    {
        if (count <= 0) return Vector2.zero;

        // 1つだけなら真横に出す
        if (count == 1)
        {
            float rad = startAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
        }

        float anglePerUnit = 360f / count;
        float angle = startAngle + anglePerUnit * index;

        if (clockwise)
            angle = -angle;

        float radian = angle * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * radius;
    }
}
