using UnityEngine;

[System.Serializable]

public class ShotType
{public AttackPatternSO attackPattern;
    public GameObject bulletPrefab;
    public float shootInterval;
    [Header("Pool‚·‚é—Ê")]
    public int poolSize = 30;
    [Header("ƒ_ƒ[ƒW")]
    public int damage = 1;
    [HideInInspector] public float shootCount;
}
