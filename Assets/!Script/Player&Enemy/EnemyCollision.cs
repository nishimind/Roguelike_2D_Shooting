using UnityEngine;

public class EnemyCollision : CollisionBase
{
    void Start() { health = GetComponent<HealthBase>(); }
}
