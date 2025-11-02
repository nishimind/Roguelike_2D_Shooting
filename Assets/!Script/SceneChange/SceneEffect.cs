using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;  


public class SceneEffect : MonoBehaviour
{
    public float fadeSpeed = 0.2f;
    public float alpha = 1f;
    public Image image;
    // Start is called before the first frame update
    public static SceneEffect Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public async UniTask FadeOut()
    {
        alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime / fadeSpeed;
            image.color = new Color(0, 0, 0, alpha);
            await UniTask.Yield();
        }
    }
    public async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            image.color = new Color(0, 0, 0, alpha);
            await UniTask.Yield();
        }
        // Instance ‚ª¶¬‚³‚ê‚é‚Ü‚Å‘Ò‚Â
        await UniTask.WaitUntil(() => Siene_Change_Main_Shooting.Instance != null);

        await UniTask.WaitUntil(() => Siene_Change_Main_Shooting.Instance.isChangingScene == true);
        FadeOut();

    }
}
