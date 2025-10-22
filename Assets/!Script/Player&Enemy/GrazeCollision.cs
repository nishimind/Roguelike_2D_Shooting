using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazeCollision : CollisionBase

{
    public AudioClip grazeClip;
    public AudioSource audio;
    public GrazeEffectPool GrazeEffectPool;
    public PlayerStatus status;
    // Start is called before the first frame update
   protected override void Start()
    {
        status = GameObject.FindWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        
    }

    // Update is called once per frame
  
        protected override void TakeDamage(Collider2D collision)
    {
        var bullet = collision.GetComponent<BulletDamage>();
        // BulletDamage がついていなければ処理を中断
        if (bullet == null) return;
        if (bullet.grazed) return;

        status.grazeCount++;
        if (grazeClip != null)
            audio.PlayOneShot(grazeClip, 0.3f);
        bullet.grazed = true;
        // パーティクルをプールから取得
        if (GrazeEffectPool != null)
            GrazeEffectPool.GetEffect(transform.position);
    }


}
