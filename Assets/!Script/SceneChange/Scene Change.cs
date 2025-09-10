using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //�V�[����������
    [SerializeField] private string[] sceneNames;


    //�{�^������������V�[����ς���
    public void ChangeSceneRandomly()
    {
        if (sceneNames.Length == 0)
        {
            Debug.LogError("�V�[�������ݒ肳��Ă��܂���I");
            return;
        }
        //�����_���ɃV�[����I��Ń��[�h����
        int randomIndex = Random.Range(0, sceneNames.Length);
         SceneManager.LoadScene(sceneNames[randomIndex]);


    }
}   

 
