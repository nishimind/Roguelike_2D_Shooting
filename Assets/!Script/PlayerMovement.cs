using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("�ړ����x")]
    public float moveSpeed = 5f;           // �ʏ푬�x
    public float slowMultiplier = 0.3f;    // �����{��

    [Header("�ړ��͈�")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    private Vector2 moveInput;             // ���͒l
    private bool isSlow = false;           // ������Ԃ��ǂ���
    private bool upPressed, downPressed, leftPressed, rightPressed;


    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Input System�ŌĂ΂��ړ�����

  /*  public void OnMoveStick(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }*/
    public void OnMoveUp(InputAction.CallbackContext context)
    {
        upPressed = context.ReadValue<float>() > 0.5f;
    }
    public void OnMoveDown(InputAction.CallbackContext context)
    {
        downPressed = context.ReadValue<float>() > 0.5f;
    }
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        leftPressed = context.ReadValue<float>() > 0.5f;
    }
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        rightPressed = context.ReadValue<float>() > 0.5f;
    }
    public void OnSlow(InputAction.CallbackContext context)
    {
        isSlow = context.ReadValue<float>() > 0.5f;
    }

    private void FixedUpdate()
    {
        // Vector2 �ɕϊ�
   //  if(upPressed||downPressed||rightPressed||leftPressed) 
            moveInput = Vector2.zero;
        if (upPressed) moveInput.y = 1;
        if (downPressed) moveInput.y= -1;
        if (rightPressed) moveInput.x = 1;
        if (leftPressed) moveInput.x = -1;

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        float currentSpeed = isSlow ? moveSpeed * slowMultiplier : moveSpeed;
        rb.velocity = moveInput * currentSpeed;
      
        ClampPosition();
    }

    // ���[���h���W�ňړ�����
    private void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
