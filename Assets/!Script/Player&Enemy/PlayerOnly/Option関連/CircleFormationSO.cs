using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Option/Formation/Circle")]
public class CircleFormationSO : FormationSO
{
    public float radius = 2f;
    public float startAngle;

    public override Vector2 GetNormalPosition(int index, int count)
    {
        float angle = startAngle + (360f / count) * index;
        float rad = angle * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
    }

    public override Vector2 GetSlowPosition(int index, int count)
    {
        // ’á‘¬Žž‚ÍŠñ‚¹‚é
        return GetNormalPosition(index, count) * 0.4f;
    }
}
