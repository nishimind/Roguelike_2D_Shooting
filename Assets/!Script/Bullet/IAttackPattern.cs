using UnityEngine;

public abstract class AttackPatternSO : ScriptableObject
{public bool isPlyerBullet = false;
    public abstract void Shoot(Vector3 position, int angle, GameObject bulletPrefab, float damage);
}