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
        {if(GetComponent<EnemyHealth>().isLastEnemy == true) Siene_Change_Main_Shooting.Instance.lastEnemyDead = true;
            Siene_Change_Main_Shooting.Instance.UnregisterEnemy(this.gameObject);

            Destroy(gameObject);
        }
    }

}
