using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //サンプルコメントあああああお
    //sampleコメントいいいいいいい
    //a
    [Header("移動速度")]
    public float moveSpeed = 5f;
    public float slowMultiplier = 0.3f;

    [Header("移動範囲")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    // 弾関連
    //[SerializeField,Header("弾オブジェクト")]
    //private GameObject _bullet;
    [SerializeField,Header("弾のプーラー")]
    private BulletPool _bulletPooler;
    [SerializeField, Header("発射する時間")]
    private float _shootTime;

    private Vector2 moveInput;            // スティック入力
    private bool isSlow;                  // 減速状態

    private Rigidbody2D rb;
    private float shootCount;

    // ボタン入力状態
    private bool upPressed, downPressed, leftPressed, rightPressed,shotPressed;

    // 最後に押した方向
    private enum LastDir { None, Up, Down, Left, Right }
    private LastDir lastVertical = LastDir.None;
    private LastDir lastHorizontal = LastDir.None;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shootCount = 0f;
    }
    
    private void Update()
    {
        //弾を撃つ処理を下に移動させました

    }
    public void OnShot(InputAction.CallbackContext context)
    {
        shotPressed = context.ReadValue<float>() > 0.5f;
      
    }
    // スティック移動
    public void OnMoveStick(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // 上ボタン
    public void OnMoveUp(InputAction.CallbackContext context)
    {
        upPressed = context.ReadValue<float>() > 0.5f;
        if (upPressed) lastVertical = LastDir.Up;
        else if (lastVertical == LastDir.Up) lastVertical = downPressed ? LastDir.Down : LastDir.None;
    }

    // 下ボタン
    public void OnMoveDown(InputAction.CallbackContext context)
    {
        downPressed = context.ReadValue<float>() > 0.5f;
        if (downPressed) lastVertical = LastDir.Down;
        else if (lastVertical == LastDir.Down) lastVertical = upPressed ? LastDir.Up : LastDir.None;
    }

    // 左ボタン
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        leftPressed = context.ReadValue<float>() > 0.5f;
        if (leftPressed) lastHorizontal = LastDir.Left;
        else if (lastHorizontal == LastDir.Left) lastHorizontal = rightPressed ? LastDir.Right : LastDir.None;
    }

    // 右ボタン
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        rightPressed = context.ReadValue<float>() > 0.5f;
        if (rightPressed) lastHorizontal = LastDir.Right;
        else if (lastHorizontal == LastDir.Right) lastHorizontal = leftPressed ? LastDir.Left : LastDir.None;
    }

    // 減速ボタン（Shift / B）
    public void OnSlow(InputAction.CallbackContext context)
    {
        isSlow = context.ReadValue<float>() > 0.5f;
    }

    private void FixedUpdate()
    {
        PlayerMove();

        ClampPosition();

        //弾を撃つ処理を移動させました, 弾発射処理を軽い方式に修正
        shootCount += Time.deltaTime;
        if (shotPressed && shootCount >= _shootTime)
        {
            _bulletPooler.Get(transform.position, transform.rotation);
            shootCount = 0f;
        }

    }
    private void PlayerMove()
    {  // ボタン入力を優先する方向に変換
        Vector2 finalInput = moveInput;

        // 垂直方向
        if (lastVertical == LastDir.Up) finalInput.y = 1;
        else if (lastVertical == LastDir.Down) finalInput.y = -1;

        // 水平方向
        if (lastHorizontal == LastDir.Right) finalInput.x = 1;
        else if (lastHorizontal == LastDir.Left) finalInput.x = -1;

        // 入力を正規化（対角移動時の速度補正）
        finalInput = Vector2.ClampMagnitude(finalInput, 1f);

        // 速度適用
        float currentSpeed = isSlow ? moveSpeed * slowMultiplier : moveSpeed;
        rb.velocity = finalInput * currentSpeed;
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
