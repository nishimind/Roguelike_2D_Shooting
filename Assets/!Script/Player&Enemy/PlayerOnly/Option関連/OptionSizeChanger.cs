using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OptionSizeChanger : MonoBehaviour
{
    [Header("Šgk”{—¦i1 = “™”{j")]
    public float sizeMultiplier = 1.2f; // Šg‘å‚Ì”{—¦
    [Header("Šgk‚É‚©‚¯‚éŠÔi1‰ñ‚ ‚½‚èj")]
    public float changeTime = 1f;

    void Start()
    {float randomtime=  Random.Range(0f,0.3f);
        var t = gameObject.transform.localScale;
        // oŒ»‚É‘å‚«‚³0‚©‚ç‚É‚ã‚Á‚Æ
        transform.localScale = Vector3.zero;
        transform.DOScale(t, 0.4f)
            .SetEase(Ease.OutBack) // ‚¿‚å‚Á‚Æ’e‚Ş‚æ‚¤‚Èu‚É‚ã‚Áv
            .OnComplete(() =>
            {
                // Šg‘åk¬‚ğ‰i‰“‚ÉŒJ‚è•Ô‚·
                transform.DOScale(t * sizeMultiplier, changeTime+ randomtime)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
    }
}
