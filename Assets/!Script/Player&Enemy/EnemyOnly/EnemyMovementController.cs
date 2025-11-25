using Cysharp.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
public class EnemyMovementController : MonoBehaviour
{
    private EnemyPhaseAttack _phaseAttack;
    protected Rigidbody2D _rb;
    protected Transform _player;
    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _phaseAttack = GetComponent<EnemyPhaseAttack>();
        if (_phaseAttack != null)
        {
            _phaseAttack.OnPhaseChanged += HandlePhaseChange;
        }
     FindPlayer();
    }

  private async UniTaskVoid FindPlayer() {   
        await UniTask.WaitUntil(() => PlayerMovement.Instance != null);
        _player = PlayerMovement.Instance.transform;
    }
   

    private void OnDestroy()
    {
        if (_phaseAttack != null)
            _phaseAttack.OnPhaseChanged -= HandlePhaseChange;
    }

    protected virtual void HandlePhaseChange(int newPhaseIndex)
    {
        Debug.Log($"移動パターン変更：Phase {newPhaseIndex}");

        switch (newPhaseIndex)
        {
            case 1:
                StartMovePattern1();
                break;
            case 2:
                StartMovePattern2();
                break;
            case 3:
                StartMovePattern3();
                break;
        }
    }

    private void StartMovePattern1() { /* 移動パターン1開始 */ }
    private void StartMovePattern2() { /* 移動パターン2開始 */ }
    private void StartMovePattern3() { /* 移動パターン3開始 */ }
}
