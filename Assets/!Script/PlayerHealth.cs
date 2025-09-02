using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
   
        public int maxHP = 10;
        public int currentHP;

        void Start()
        {
            currentHP = maxHP;
        }

        public void TakeDamage(int damage)
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                Die();
            }
        }

        void Die()
        {
            // ゲームオーバー処理など
            Debug.Log("Player Dead!");
        }
    
}
