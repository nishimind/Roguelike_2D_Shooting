using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackSet
{
    [Header("攻撃パターン（ScriptableObject）")]
    public AttackPatternSO attackPattern;

    [Header("発射間隔(秒) ー 2発目以降")]
    public float shootInterval = 1f;

    [Header("使用する弾プレハブ")]
    public GameObject bulletPrefab;

    [Header("発射角度")]
    public int shootAngle;

    [Header("最初の発射までの遅延(秒)")]
    public float initialDelay = 0f;

    [Header("この攻撃を使用するHP範囲（％）")]
    public int startHP = 100;
    public int endHP = 0;

    [Header("ダメージ")]
    public float damage = 1f;

    [Header("Poolする量")]
    public int poolSize = 30;

    // ---- 新機能：バースト発射 + インターバル ----
    [Header("何発撃ったらインターバル発生するか")]
    public int burstCount = 0; // 0なら無効

    [Header("インターバル時間(秒)")]
    public float burstInterval = 0f; // 0なら無効

    // ランタイム変数
    [HideInInspector] public float shootTimer = 0f;
    [HideInInspector] public bool firstShotDone = false;

    [HideInInspector] public int currentShotCount = 0;
    [HideInInspector] public bool inBurstCooldown = false;
    [HideInInspector] public float burstTimer = 0f;
}