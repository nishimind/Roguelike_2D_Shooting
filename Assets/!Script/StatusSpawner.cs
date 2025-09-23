using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSpawner : MonoBehaviour
{
    public GameObject playerStatus;
    public static PlayerStatus Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.FindWithTag("PlayerStatus") == null)
        {

            DontDestroyOnLoad(playerStatus);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
