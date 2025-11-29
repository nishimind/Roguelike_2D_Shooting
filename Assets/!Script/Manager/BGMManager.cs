using UnityEngine;

public class BGMManager_DSP : MonoBehaviour
{
    [Header("1曲目（イントロ）")]
    public AudioClip introClip;

    [Header("2曲目（メインループ）")]
    public AudioClip loopClip;

    private AudioSource introSource;
    private AudioSource loopSource;

    void Start()
    {
        // AudioSource を2つ用意
        introSource = gameObject.AddComponent<AudioSource>();
        loopSource = gameObject.AddComponent<AudioSource>();

        introSource.clip = introClip;
        introSource.loop = false;

        loopSource.clip = loopClip;
        loopSource.loop = true;

        PlayIntroThenLoop_DSP();
    }

    private void PlayIntroThenLoop_DSP()
    {
        // 今のDSP時間を取得
        double dspNow = AudioSettings.dspTime;

        // ちょっとだけ先の時間から再生開始（余裕を持たせる）
        double introStartTime = dspNow + 0.1f;

        // イントロを予約再生
        introSource.PlayScheduled(introStartTime);

        // イントロの長さ（秒）
        double introLength = (double)introClip.samples / introClip.frequency;
        // ※ introClip.length でもOKだが、samples/frequency の方がより正確

        // ループ曲の開始時間 = イントロ開始時間 + イントロの長さ
        double loopStartTime = introStartTime + introLength;

        // ループ曲を「イントロが終わるぴったりの時間」に予約再生
        loopSource.PlayScheduled(loopStartTime);
    }
}