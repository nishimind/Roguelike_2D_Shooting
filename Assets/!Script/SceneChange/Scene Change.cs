using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //ボタンを押したらシーンを変える
    public void Change_bitton()
    {
        SceneManager.LoadScene("Main_Shooting");
    }
}
