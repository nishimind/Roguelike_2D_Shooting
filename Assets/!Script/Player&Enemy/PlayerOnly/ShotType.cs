using UnityEngine;

[System.Serializable]

public class ShotType
{public AttackPatternSO attackPattern;
    public GameObject bulletPrefab;
    public float shootInterval;
    [Header("Poolする量")]
    public int poolSize = 30;
    [Header("ダメージ")]
    public int damage = 1;
    [Header("発射角度")]
    public int shootAngle ;
    [HideInInspector] public float shootCount;
}
