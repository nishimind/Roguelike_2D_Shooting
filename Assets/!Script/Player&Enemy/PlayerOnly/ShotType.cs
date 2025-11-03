using UnityEngine;

[System.Serializable]

public class ShotType
{public AttackPatternSO attackPattern;
    public GameObject bulletPrefab;
    public float shootInterval;

    [HideInInspector] public float shootCount;
}
