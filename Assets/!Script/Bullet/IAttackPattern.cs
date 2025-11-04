using UnityEngine;

public abstract class AttackPatternSO : ScriptableObject
{
    public abstract void Shoot(GameObject shootPotision, GameObject bulletPrefab);
}