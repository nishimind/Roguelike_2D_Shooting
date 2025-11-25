using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/‰º‚É‚Ü‚Á‚·‚®i‚Ş")]
public class Straight_Down : MovePatternSO
{
    public float speed = 2f;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
    

        // ‰i‘±“I‚É‰º•ûŒü‚ÉˆÚ“®
        while (controller != null)
        {
            controller._rb.velocity = Vector2.down * speed;
            await UniTask.Yield();
        }
    }
}
