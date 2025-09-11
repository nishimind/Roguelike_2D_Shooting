using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //シーン名を入れる
    [SerializeField] private string[] sceneNames;


    //ボタンを押したらシーンを変える
    public void ChangeSceneRandomly()
    {
        if (sceneNames.Length == 0)
        {
            Debug.LogError("シーン名が設定されていません！");
            return;
        }
        //ランダムにシーンを選んでロードする
        int randomIndex = Random.Range(0, sceneNames.Length);
         SceneManager.LoadScene(sceneNames[randomIndex]);


    }
}   

 
