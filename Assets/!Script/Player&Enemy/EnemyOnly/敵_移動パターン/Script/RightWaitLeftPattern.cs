using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/RightWaitLeft")]
public class RightWaitLeftPattern : MovePatternSO
{
    public float speed = 3f;
    public float waitTime = 1f;
    public float moveDuration = 2f;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        var rb = controller._rb;

        // ‡@ ‰E‚ÖˆÚ“®
        rb.velocity = Vector2.right * speed;
        await UniTask.Delay((int)(moveDuration * 1000));

        // ‡A ­‚µ‘Ò‚Â
        rb.velocity = Vector2.zero;
        await UniTask.Delay((int)(waitTime * 1000));

        // ‡B ¶‚ÖˆÚ“®
        rb.velocity = Vector2.left * speed;
        await UniTask.Delay((int)(moveDuration * 1000));

        // ‡C ÅŒã‚É’â~
        rb.velocity = Vector2.zero;
    }
}
