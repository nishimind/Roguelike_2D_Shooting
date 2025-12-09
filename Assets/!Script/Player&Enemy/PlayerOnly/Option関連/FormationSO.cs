using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Option/Formation")]
public class FormationSO : ScriptableObject
{
    public AnimationCurve xCurve;  // index 0 Å® 1 ÇÃà íu
    public AnimationCurve yCurve;

    public float size = 1f;

   protected virtual  Vector2 GetPosition(int index, int count)
    {
        return  Vector2.zero;
    }
}
