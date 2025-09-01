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

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Input System�ŌĂ΂��ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Input System�ŌĂ΂�錸������
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

    // ���[���h���W�ňړ�����
    private void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
