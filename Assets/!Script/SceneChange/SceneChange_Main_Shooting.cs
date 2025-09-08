using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Siene_Change_Main_Shooting : MonoBehaviour
{
    private GameObject[] enemyBox;
    private GameObject[] player;
    // Start is called before the first frame update
    
    //二重に呼ばれないようにフラグを追加
    private bool isChangingScene = false;

    // Update is called once per frame
    void Update()
    {
        // 既に切り替え中なら処理しない 
     //   if (!isChangingScene) return;

        //Enemyタグのついたオブジェクトをすべて取得
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectsWithTag("Player");
        

        print("敵の数:" + enemyBox.Length);
        print("プレイヤーの数:" + player.Length);

        //敵が全滅したらシーンを切り替える
        if (enemyBox.Length == 0)
        {
            StartCoroutine(ChangeSceneWithDelay("Shop", 2f));

        }
        //プレイヤーが全滅したらシーンを切り替える
        if (player.Length == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        IEnumerator ChangeSceneWithDelay(string sceneName, float delay)
        {
            isChangingScene = true; // フラグを立てて多重呼び出しを防ぐ
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(sceneName);
        }

    }

}