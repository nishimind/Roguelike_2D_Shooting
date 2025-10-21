using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazeCollision : CollisionBase

{
    public AudioClip grazeClip;
    public AudioSource audio;
    public GrazeEffectPool GrazeEffectPool;
    // Start is called before the first frame update
   protected override void Start()
    {
        
    }

    // Update is called once per frame
  
        protected override void TakeDamage(Collider2D collision)
    {
        if (grazeClip != null)
            audio.PlayOneShot(grazeClip, 0.3f);

        // パーティクルをプールから取得
        if (GrazeEffectPool != null)
            GrazeEffectPool.GetEffect(transform.position);
    }


}
