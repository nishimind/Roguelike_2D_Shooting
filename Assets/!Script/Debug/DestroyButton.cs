using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : MonoBehaviour
{
    public void DestroyAllEnemies()
    {
        // "Enemy" ƒ^ƒO‚ª•t‚¢‚½‘S‚Ä‚Ì“G‚ðŽæ“¾
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ‘S‚Ä‚Ì“G‚ð”j‰ó
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
