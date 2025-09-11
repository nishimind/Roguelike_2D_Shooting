using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("遅くなる時間")]
    private float deadEffectTimeScale;
    [SerializeField, Header("遅くなる時間の長さ")]
    private float deadEffectTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DeadEffect()
    {
        StartCoroutine(Slow());
    }

    //時間を遅くする
    IEnumerator Slow()
    {
        Time.timeScale = deadEffectTimeScale;　//ゲーム内の経過速度を変更できる
        yield return new WaitForSecondsRealtime(deadEffectTime);　//指定した秒数待つ
        Time.timeScale = 1f;
    }
}
