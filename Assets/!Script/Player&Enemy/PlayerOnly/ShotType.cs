using UnityEngine;

[System.Serializable]

public class ShotType
{public AttackPatternSO attackPattern;
    public GameObject bulletPrefab;
    public float shootInterval;
    [Header("Pool‚·‚é—Ê")]
    public int poolSize = 30;
    [HideInInspector] public float shootCount;
}
