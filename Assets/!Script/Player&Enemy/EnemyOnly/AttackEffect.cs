using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// オブジェクトが「点滅 → 赤固定 → 元に戻る」を行う演出
/// </summary>
public class AttackEffect : MonoBehaviour
{
    [Header("点滅するまでの待機時間")]
    public float startDelay = 0f;

    [Header("点滅している時間")]
    public float blinkDuration = 0.5f;

    [Header("赤く光る時間")]
    public float flashDuration = 0.5f;

    [Header("点滅の間隔（秒）")]
    public float blinkInterval = 0.1f;

    [Header("点滅色")]
    public Color blinkColor = Color.red;

    [Header("赤く光る色")]
    public Color flashColor = new Color(1f, 0.3f, 0.3f);

    SpriteRenderer[] renderers;
    Color[] originalColors;

    void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].color;
    }

    /// <summary>
    /// メイン演出を実行
    /// </summary>
    public async UniTask Play()
    {
        // ① 開始待機
        if (startDelay > 0)
            await UniTask.Delay((int)(startDelay * 1000));

        // ② 点滅フェーズ
        float t = 0;
        while (t < blinkDuration)
        {
            SetColor(blinkColor);
            await UniTask.Delay((int)(blinkInterval * 1000));

            RestoreColor();
            await UniTask.Delay((int)(blinkInterval * 1000));

            t += blinkInterval * 2;
        }

        // ③ 赤く光るフェーズ
        SetColor(flashColor);
        await UniTask.Delay((int)(flashDuration * 1000));

        // ④ 元の色に戻す
        RestoreColor();
    }

    private void SetColor(Color c)
    {
        foreach (var r in renderers)
            r.color = c;
    }

    private void RestoreColor()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].color = originalColors[i];
    }
}