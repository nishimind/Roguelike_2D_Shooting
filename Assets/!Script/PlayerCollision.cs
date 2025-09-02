using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BulletDamage bullet = other.GetComponent<BulletDamage>();
        if (bullet != null)
        {
            playerHealth.TakeDamage(bullet.damage);
           if(bullet.ifdestroied) Destroy(other.gameObject); // íeÇè¡Ç∑
        }
    }
}
