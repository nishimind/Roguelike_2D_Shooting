using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/‚»‚Ìê‚Å’â~(–³ŒÀ)")]
public class Stay_Stop_Infinite : MovePatternSO
{
    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        if (controller == null) return;

        Rigidbody2D rb = controller._rb;

        // ¥ Å‰‚ÉŠ®‘S’â~
        rb.velocity = Vector2.zero;

        while (controller != null)
        {
            // ¥ í‚É’â~ó‘Ô‚ğˆÛ
            rb.velocity = Vector2.zero;

            await UniTask.Yield();
        }
    }
}