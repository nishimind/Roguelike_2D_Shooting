using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Option/Formation/Base")]
public class FormationSO : ScriptableObject
{
    public virtual Vector2 GetNormalPosition(int index, int count)
    {
        return Vector2.zero;
    }

    public virtual Vector2 GetSlowPosition(int index, int count)
    {
        return GetNormalPosition(index, count);
    }
}
