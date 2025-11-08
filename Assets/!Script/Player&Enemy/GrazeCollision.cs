using UnityEngine;

public class GrazeCollision : CollisionBase
{
    public AudioClip grazeClip;
    public AudioSource audio;
    public GrazeEffectPool GrazeEffectPool;
    public PlayerStatus status;
    public CollisionBase mainCollision; // ← メイン当たり判定をインスペクターで指定

    void Start()
    {
        status = PlayerStatus.Instance;
    }

    protected override void TakeDamage(Collider2D collision)
    {
        // --- メインの当たり判定にも触れていたらスキップ ---
        if (mainCollision != null && mainCollision.stayingColliders.Contains(collision))
            return;

        var bullet = collision.GetComponent<BulletDamage>();
        if (bullet == null) return;
        if (bullet.grazed) return;

        status.grazeCount++;
        if (grazeClip != null)
            audio.PlayOneShot(grazeClip, 0.3f);

        if (bullet.onlyGraze)
            bullet.grazed = true;

        if (GrazeEffectPool != null)
            GrazeEffectPool.GetEffect(transform.position);
    }
}
