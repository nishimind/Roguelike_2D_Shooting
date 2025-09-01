using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 5f;           // 通常速度
    public float slowMultiplier = 0.3f;    // 減速倍率

    [Header("移動範囲")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    private Vector2 moveInput;             // 入力値
    private bool isSlow = false;           // 減速状態かどうか

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Input Systemで呼ばれる移動入力
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Input Systemで呼ばれる減速入力
    public void OnSlow(InputAction.CallbackContext context)
    {
        isSlow = context.ReadValue<float>() > 0.5f;
    }

    private void FixedUpdate()
    {
        float currentSpeed = isSlow ? moveSpeed * slowMultiplier : moveSpeed;
        Vector2 velocity = moveInput * currentSpeed;
        rb.velocity = velocity;

        ClampPosition();
    }

    // ワールド座標で移動制限
    private void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
