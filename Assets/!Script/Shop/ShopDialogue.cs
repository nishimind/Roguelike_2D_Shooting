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

    [Header("Messages")]
    [SerializeField] [TextArea] private List<string> messages = new();

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);

        // ƒ‰ƒ“ƒ_ƒ€‚É1‚Â‘I‚Ô
        if (messages.Count > 0)
        {
            string msg = messages[Random.Range(0, messages.Count)];
            StartTyping(msg);
        }
    }

    public void StartTyping(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeRoutine(message));
    }

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
