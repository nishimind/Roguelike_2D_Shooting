using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("�x���Ȃ鎞��")]
    private float deadEffectTimeScale;
    [SerializeField, Header("�x���Ȃ鎞�Ԃ̒���")]
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

    //���Ԃ�x������
    IEnumerator Slow()
    {
        Time.timeScale = deadEffectTimeScale;�@//�Q�[�����̌o�ߑ��x��ύX�ł���
        yield return new WaitForSecondsRealtime(deadEffectTime);�@//�w�肵���b���҂�
        Time.timeScale = 1f;
    }
}
