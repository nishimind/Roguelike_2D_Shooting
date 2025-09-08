using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Siene_Change_Main_Shooting : MonoBehaviour
{
    private GameObject[] enemyBox;
    private GameObject[] player;
    // Start is called before the first frame update
    
    //��d�ɌĂ΂�Ȃ��悤�Ƀt���O��ǉ�
    private bool isChangingScene = false;

    // Update is called once per frame
    void Update()
    {
        // ���ɐ؂�ւ����Ȃ珈�����Ȃ� 
     //   if (!isChangingScene) return;

        //Enemy�^�O�̂����I�u�W�F�N�g�����ׂĎ擾
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectsWithTag("Player");
        

        print("�G�̐�:" + enemyBox.Length);
        print("�v���C���[�̐�:" + player.Length);

        //�G���S�ł�����V�[����؂�ւ���
        if (enemyBox.Length == 0)
        {
            StartCoroutine(ChangeSceneWithDelay("Shop", 2f));

        }
        //�v���C���[���S�ł�����V�[����؂�ւ���
        if (player.Length == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        IEnumerator ChangeSceneWithDelay(string sceneName, float delay)
        {
            isChangingScene = true; // �t���O�𗧂Ăđ��d�Ăяo����h��
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(sceneName);
        }

    }

}