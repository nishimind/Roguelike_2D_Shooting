using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyoutDestroy : MonoBehaviour
{
    private bool hasAppeared = false;

    void OnBecameVisible()
    {
        hasAppeared = true;
    }
    // Start is called before the first frame update

    private void OnBecameInvisible()
    {
        if(hasAppeared)
        {
            Destroy(gameObject);
        }
    }

}
