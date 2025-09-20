using UnityEngine;

public abstract class AttackPatternSO : ScriptableObject
{
    public abstract void Shoot(Enemy enemy);
}