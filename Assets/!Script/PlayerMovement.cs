using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("�ړ����x")]
    public float moveSpeed = 5f;
    public float slowMultiplier = 0.2f;

    [Header("�ړ��͈�")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    private Vector2 moveInput;            // �X�e�B�b�N����
    private bool isSlow;                  // �������

    private Rigidbody2D rb;

    // �{�^�����͏��
    private bool upPressed, downPressed, leftPressed, rightPressed;

    // �Ō�ɉ���������
    private enum LastDir { None, Up, Down, Left, Right }
    private LastDir lastVertical = LastDir.None;
    private LastDir lastHorizontal = LastDir.None;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // �X�e�B�b�N�ړ�
    public void OnMoveStick(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // ��{�^��
    public void OnMoveUp(InputAction.CallbackContext context)
    {
        upPressed = context.ReadValue<float>() > 0.5f;
        if (upPressed) lastVertical = LastDir.Up;
        else if (lastVertical == LastDir.Up) lastVertical = downPressed ? LastDir.Down : LastDir.None;
    }

    // ���{�^��
    public void OnMoveDown(InputAction.CallbackContext context)
    {
        downPressed = context.ReadValue<float>() > 0.5f;
        if (downPressed) lastVertical = LastDir.Down;
        else if (lastVertical == LastDir.Down) lastVertical = upPressed ? LastDir.Up : LastDir.None;
    }

    // ���{�^��
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        leftPressed = context.ReadValue<float>() > 0.5f;
        if (leftPressed) lastHorizontal = LastDir.Left;
        else if (lastHorizontal == LastDir.Left) lastHorizontal = rightPressed ? LastDir.Right : LastDir.None;
    }

    // �E�{�^��
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        rightPressed = context.ReadValue<float>() > 0.5f;
        if (rightPressed) lastHorizontal = LastDir.Right;
        else if (lastHorizontal == LastDir.Right) lastHorizontal = leftPressed ? LastDir.Left : LastDir.None;
    }

    // �����{�^���iShift / B�j
    public void OnSlow(InputAction.CallbackContext context)
    {
        isSlow = context.ReadValue<float>() > 0.5f;
    }

    private void FixedUpdate()
    {
        // �{�^�����͂�D�悷������ɕϊ�
        Vector2 finalInput = moveInput;

        // ��������
        if (lastVertical == LastDir.Up) finalInput.y = 1;
        else if (lastVertical == LastDir.Down) finalInput.y = -1;

        // ��������
        if (lastHorizontal == LastDir.Right) finalInput.x = 1;
        else if (lastHorizontal == LastDir.Left) finalInput.x = -1;

        // ���͂𐳋K���i�Ίp�ړ����̑��x�␳�j
        finalInput = Vector2.ClampMagnitude(finalInput, 1f);

        // ���x�K�p
        float currentSpeed = isSlow ? moveSpeed * slowMultiplier : moveSpeed;
        rb.velocity = finalInput * currentSpeed;

        ClampPosition();
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
