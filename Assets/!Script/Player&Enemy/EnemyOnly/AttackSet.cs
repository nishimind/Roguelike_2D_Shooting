using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AttackSet
{
    [Header("攻撃パターン（ScriptableObject）")]
    public AttackPatternSO attackPattern;

    [Header("発射間隔(秒)")]
    public float shootInterval = 1f;
    [Header("使用する弾プレハブ")]
    public GameObject bulletPrefab;
    [Header("発射角度")]
    public int shootAngle ;
    [Header("最初の発射までの遅延(秒)")]
    public int initialDelay = 0;
    [Header("ダメージ")]
    public int damage = 1;
    [Header("Poolする量")]
    public int poolSize = 30;


    [HideInInspector] public float shootTimer = 0f;
}