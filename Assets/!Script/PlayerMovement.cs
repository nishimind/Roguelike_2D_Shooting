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
    public void OnMoveStick(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnMoveUp(InputAction.CallbackContext context)
    {
        // ボタンが押されている間だけ y に加算
        moveInput.y = context.ReadValue<float>(); // 1 if pressed, 0 if not
    }
    public void OnMoveDown(InputAction.CallbackContext context)
    {
        moveInput.y = -context.ReadValue<float>();
    }
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        moveInput.x = context.ReadValue<float>();
    }
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        moveInput.x = -context.ReadValue<float>();
    }

    // Input Systemで呼ばれる減速入力
    public void OnSlow(InputAction.CallbackContext context)
    {
        isSlow = true;
    }

    private void FixedUpdate()
    {
        moveInput.x = Mathf.Clamp(moveInput.x, -1f, 1f);
        moveInput.y = Mathf.Clamp(moveInput.y, -1f, 1f);

        float currentSpeed = isSlow ? moveSpeed * slowMultiplier : moveSpeed;
        Vector2 velocity = moveInput * currentSpeed;
        rb.velocity = velocity;
        Debug.Log(moveInput);
        isSlow = false;
        //moveInput=Vector2.zero ;
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
