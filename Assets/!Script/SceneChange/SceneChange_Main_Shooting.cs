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
        //Enemy�^�O�̂����I�u�W�F�N�g�����ׂĎ擾
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectsWithTag("Player");
        

        print("�G�̐�:" + enemyBox.Length);
        print("�v���C���[�̐�:" + player.Length);

        //�G���S�ł�����V�[����؂�ւ���
        if (enemyBox.Length == 0)
        {
            SceneManager.LoadScene("Shop");
        }
        //�v���C���[���S�ł�����V�[����؂�ւ���
        if (player.Length == 0)
        {
            SceneManager.LoadScene("GsmeOver");
        }
    }
}