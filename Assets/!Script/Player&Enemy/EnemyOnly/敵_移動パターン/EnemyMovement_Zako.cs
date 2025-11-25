using UnityEngine;

public class EnemyMovement_Zako : EnemyMovementController
{
  
 
    [Header("Move Settings")]
    public float moveSpeed = 2f;
    public float zigzagAmplitude = 1.5f;   // ジグザグの幅
    public float zigzagSpeed = 3f;         // ジグザグの速さ
    public float homingPower = 2f;         // ホーミングの曲がりやすさ

    private float _time;

   

    private void Update()
    {
        _time += Time.deltaTime;
        Move();
    }

    private enum MoveType { Straight, Zigzag, Homing }
    private MoveType _currentMoveType = MoveType.Straight;

    // ==========================================
    // フェーズ切り替えイベント処理
    // ==========================================
    protected override void HandlePhaseChange(int newPhase)
    {
        base.HandlePhaseChange(newPhase);

        switch (newPhase)
        {
            case 1:
                SetMoveStraight();
                break;
            case 2:
                SetMoveZigzag();
                break;
            case 3:
                SetMoveHoming();
                break;
        }
    }

    // ==========================================
    // パターン設定
    // ==========================================

    private void SetMoveStraight()
    {
        _currentMoveType = MoveType.Straight;
    }

    private void SetMoveZigzag()
    {
        _currentMoveType = MoveType.Zigzag;
        _time = 0;
    }

    private void SetMoveHoming()
    {
        _currentMoveType = MoveType.Homing;
    }

    // ==========================================
    // 実際の移動処理
    // ==========================================
    private void Move()
    {
        switch (_currentMoveType)
        {
            case MoveType.Straight:
                MoveStraight();
                break;
            case MoveType.Zigzag:
                MoveZigzag();
                break;
            case MoveType.Homing:
                MoveHoming();
                break;
        }
    }

    private void MoveStraight()
    {
        _rb.velocity = Vector2.down * moveSpeed;
    }

    private void MoveZigzag()
    {
        float x = Mathf.Sin(_time * zigzagSpeed) * zigzagAmplitude;
        _rb.velocity = new Vector2(x, -moveSpeed);
    }

    private void MoveHoming()
    {
        if (_player == null)
        {
            MoveStraight();
            return;
        }

        Vector2 dir = (_player.position - transform.position).normalized;
        Vector2 v = Vector2.Lerp(_rb.velocity, dir * moveSpeed, Time.deltaTime * homingPower);
        _rb.velocity = v;
    }
}
