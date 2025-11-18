using UnityEngine;
using UnityEngine.InputSystem; 
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public static bool IsPaused { get; private set; }

    [SerializeField] GameObject pauseUI;
    private void Awake()
    {
        pauseUI.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   public void PauseButtonDown(InputAction.CallbackContext context) 
    { 
        if (context.performed) {
            if (IsPaused) {
                Resume(); 
            } 
            else { Pause(); }
        }
    }


    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
    }
}
