using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

// --- LaserNormal ---
public class LaserNormal : BulletBase
{
    [Header("演出時間設定")]
    public float preLaserTime = 0.5f;
    public float laserTime = 2f;

    [Header("スケール設定")]
    public float preLineTargetScaleX = 1.0f;
    public float laserTargetScaleX = 1.5f;
    public float scaleDuration = 0.3f;

    [Header("オブジェクト参照")]
    public GameObject preLine;
    public GameObject laserLine;

    protected override void Initialize()
    {
       // preLine.SetActive(false);
        laserLine.SetActive(false);
        preLine.transform.localScale = new Vector3(0f, preLine.transform.localScale.y, preLine.transform.localScale.z);
        laserLine.transform.localScale = new Vector3(0f, laserLine.transform.localScale.y, laserLine.transform.localScale.z);
    }

    public async UniTask LaserSequenceAsync()
    {
        Debug.Log($"[Laser] preLine={preLine}, activeSelf={preLine.activeSelf}, preLaserTime={preLaserTime}");

        preLine.transform.DOKill();
        laserLine.transform.DOKill();

        preLine.SetActive(true);
        preLine.transform.DOScaleX(preLineTargetScaleX, scaleDuration).SetEase(Ease.OutQuad);
        await UniTask.Delay((int)(preLaserTime * 1000));

        preLine.transform.DOScaleX(0f, scaleDuration).SetEase(Ease.InQuad);
        await UniTask.Delay((int)(scaleDuration * 1000));
        preLine.SetActive(false);

        laserLine.SetActive(true);
        laserLine.transform.DOScaleX(laserTargetScaleX, scaleDuration).SetEase(Ease.OutQuad);
        await UniTask.Delay((int)(laserTime * 1000));

        laserLine.transform.DOScaleX(0f, scaleDuration).SetEase(Ease.InQuad);
        await UniTask.Delay((int)(scaleDuration * 1000));
        laserLine.SetActive(false);
    }

    protected override void Update() { }
}

