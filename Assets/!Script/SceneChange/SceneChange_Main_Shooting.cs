using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Siene_Change_Main_Shooting : MonoBehaviour
{
    private GameObject[] enemyBox;
    private GameObject[] player;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        //Enemyタグのついたオブジェクトをすべて取得
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectsWithTag("Player");
        

        print("敵の数:" + enemyBox.Length);
        print("プレイヤーの数:" + player.Length);

        //敵が全滅したらシーンを切り替える
        if (enemyBox.Length == 0)
        {
            SceneManager.LoadScene("Shop");
        }
        //プレイヤーが全滅したらシーンを切り替える
        if (player.Length == 0)
        {
            SceneManager.LoadScene("GsmeOver");
        }
    }
}