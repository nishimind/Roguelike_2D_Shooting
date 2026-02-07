using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float blinkDuration = 1.5f;
    public float blinkInterval = 0.15f;
    public Color blinkColor = Color.red;

    SpriteRenderer[] _renderers;
    Color[] _original;
    CancellationTokenSource _cts;

    void Awake()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _original = new Color[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
            _original[i] = _renderers[i].color;
    }

    public async UniTask PlayAsync()
    {
        ForceStop();
        _cts = new CancellationTokenSource();

        float t = 0;
        try
        {
            while (t < blinkDuration)
            {
                SetColor(blinkColor);
                await UniTask.Delay((int)(blinkInterval * 1000), cancellationToken: _cts.Token);

                Restore();
                await UniTask.Delay((int)(blinkInterval * 1000), cancellationToken: _cts.Token);

                t += blinkInterval * 2;
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            Restore();
        }
    }

    public void ForceStop()
    {
        _cts?.Cancel();
        Restore();
    }

    void SetColor(Color c)
    {
        foreach (var r in _renderers)
            r.color = c;
    }

    void Restore()
    {
        for (int i = 0; i < _renderers.Length; i++)
            _renderers[i].color = _original[i];
    }
}