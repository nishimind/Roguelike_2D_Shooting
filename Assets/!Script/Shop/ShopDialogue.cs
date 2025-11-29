using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopDialogue : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typeSpeed = 0.05f;

    [Header("Type Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSE;

    [Header("Start Message")]
    [SerializeField] private string startMessage = "そろそろブラックフライデーおわりますよ";

    // シーン開始時に自動で喋る
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);  // 少し待つ演出
        StartTyping(startMessage);
    }

    // 外からも呼び出せる
    public void StartTyping(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeRoutine(message));
    }

    // タイピング処理
    private IEnumerator TypeRoutine(string message)
    {
        dialogueText.text = "";

        foreach (char c in message)
        {
            dialogueText.text += c;

            if (typeSE != null && audioSource != null)
            {
                audioSource.PlayOneShot(typeSE);
            }

            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
