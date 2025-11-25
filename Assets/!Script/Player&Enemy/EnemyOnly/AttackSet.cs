using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AttackSet
{
    [Header("UŒ‚ƒpƒ^[ƒ“iScriptableObjectj")]
    public AttackPatternSO attackPattern;

    [Header("”­ËŠÔŠu(•b)")]
    public float shootInterval = 1f;
    [Header("g—p‚·‚é’eƒvƒŒƒnƒu")]
    public GameObject bulletPrefab;
    [Header("”­ËŠp“x")]
    public int shootAngle ;
    [Header("Å‰‚Ì”­Ë‚Ü‚Å‚Ì’x‰„(•b)")]
    public int initialDelay = 0;
    public int startHP = (int)100f;
    public int endHP = (int)0f;
    public float damage = 1;
    [Header("Pool‚·‚é—Ê")]
    public int poolSize = 30;


    [HideInInspector] public float shootTimer = 0f;
}