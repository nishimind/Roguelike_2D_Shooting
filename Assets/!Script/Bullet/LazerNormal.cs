using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LaserNormal : BulletBase
{
    public float preLaserTime = 0.5f;
    public float laserTime = 2f;

    public GameObject preLine;
    public GameObject laserLine;

    CancellationTokenSource _cts;
    bool _stopped;

    public async UniTask LaserSequenceAsync()
    {
        _stopped = false;
        _cts = new CancellationTokenSource();

        try
        {
            preLine.SetActive(true);
            await UniTask.Delay((int)(preLaserTime * 1000), cancellationToken: _cts.Token);

            laserLine.SetActive(true);
            await UniTask.Delay((int)(laserTime * 1000), cancellationToken: _cts.Token);
        }
        catch (OperationCanceledException) { }
        finally
        {
            Cleanup();
        }
    }

    public void ForceStopLaser()
    {
        if (_stopped) return;
        _stopped = true;

        _cts?.Cancel();
        Cleanup();
    }

    void Cleanup()
    {
        preLine.SetActive(false);
        laserLine.SetActive(false);

        var dmg = GetComponentInChildren<BulletDamage>();
        if (dmg != null)
            BulletPool.Instance.Release(dmg.originPrefab, gameObject);
    }
}
