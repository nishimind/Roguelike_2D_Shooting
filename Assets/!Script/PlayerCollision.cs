using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
   [SerializeField] private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            BulletDamage bullet = collision.GetComponent<BulletDamage>();
            if (bullet != null)
            {
                playerHealth.TakeDamage(bullet.damage);
                if (bullet.ifdestroied) collision.GetComponent<Camera_Chacker>()._pool.Release(bullet.gameObject); // íeÇè¡Ç∑
            }
        }
    }
}
