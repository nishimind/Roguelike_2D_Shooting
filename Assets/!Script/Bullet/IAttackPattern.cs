using UnityEngine;

public abstract class AttackPatternSO : ScriptableObject
{public bool isPlyerBullet = false;
    public abstract void Shoot(GameObject shootPotision, GameObject bulletPrefab,int damage);
}