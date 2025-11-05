using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionShooter : MonoBehaviour
{
    public ShotType shotType;
    // Start is called before the first frame update

    private void Start()
    {
        if (shotType.bulletPrefab != null)
            BulletPool.Instance.RegisterBulletPrefab(shotType.bulletPrefab, shotType.poolSize);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (BulletPool.Instance != null && PlayerMovement.Instance.shotPressed)
        {
            //List‚É‚µ‚½‚¢
         
                shotType.shootCount += Time.deltaTime;
                if (shotType.shootCount >= shotType.shootInterval)
                {

                    // GameObject bullet=
                   // Debug.Log(shotType.damage);
                    shotType.attackPattern.Shoot(this.gameObject.transform.position, shotType.shootAngle, shotType.bulletPrefab, shotType.damage);
                    // bullet.GetComponent<BulletDamage>().damage= PlayerStatus.Instance.attackPower;
                    shotType.shootCount = 0f;



                
         }
        }
    }
}
