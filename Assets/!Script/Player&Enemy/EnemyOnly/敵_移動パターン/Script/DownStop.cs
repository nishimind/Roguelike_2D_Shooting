using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "MovePattern/â∫Ç…à⁄ìÆÇµÇƒí‚é~")]
public class Straight_Down_And_Stop : MovePatternSO
{
    [Header("â∫ç~ë¨ìx")]
    public float speed = 2f;

    [Header("Ç«ÇøÇÁÇ©ÇÃèåèÇ≈í‚é~Ç∑ÇÈ")]
    public float moveDistance = 3f;    // Ç±ÇÃãóó£Ç‘ÇÒà⁄ìÆÇµÇΩÇÁí‚é~
    public float moveTime = 2f;        // Ç±ÇÃïbêîà⁄ìÆÇµÇΩÇÁí‚é~

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        if (controller == null) return;

        Rigidbody2D rb = controller._rb;
        Vector3 startPos = controller.transform.position;

        float timer = 0f;

        while (controller != null)
        {
            // Å• â∫ï˚å¸Ç÷à⁄ìÆ
            rb.velocity = Vector2.down * speed;

            timer += Time.deltaTime;

            // Å• í‚é~èåèÉ`ÉFÉbÉN
            float traveled = Vector3.Distance(startPos, controller.transform.position);

            if (traveled >= moveDistance || timer >= moveTime)
                break;

            await UniTask.Yield();
        }

        // Å• í‚é~
        if (controller != null)
        {
            controller._rb.velocity = Vector2.zero;
        }
    }
}