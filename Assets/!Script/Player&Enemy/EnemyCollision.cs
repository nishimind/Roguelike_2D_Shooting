using UnityEngine;

public class EnemyCollision : CollisionBase
{
    public bool hasAppeared = false;
    void OnBecameVisible()
    {
        hasAppeared = true;
    }
    void Start() {
        
        
        
        health = GetComponent<HealthBase>();
    }
    protected override void TakeDamage(Collider2D collision)
    {
      if(!hasAppeared)return;
        base.TakeDamage(collision);
    }
}
