using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void Change_bitton()
    {
        SceneManager.LoadScene("Main_Shooting");
    }
}
