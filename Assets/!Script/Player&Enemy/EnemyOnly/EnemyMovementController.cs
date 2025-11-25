using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private EnemyPhaseAttack _phaseAttack;
    public MovePatternSO[] phaseMovePatterns;

  

   [HideInInspector] public Rigidbody2D _rb;
    public Transform Player => _player;
    protected Transform _player;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _phaseAttack = GetComponent<EnemyPhaseAttack>();

        if (_phaseAttack != null)
            _phaseAttack.OnPhaseChanged += HandlePhaseChange;

        FindPlayer().Forget();

        // 最初のフェーズを開始
        StartFirstPhase();
    }

    private void StartFirstPhase()
    {
        if (phaseMovePatterns.Length > 0)
            phaseMovePatterns[0]?.Execute(this).Forget();
    }


    private async UniTaskVoid FindPlayer()
    {
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

        if (newPhaseIndex < 0 || newPhaseIndex >= phaseMovePatterns.Length) return;

        var pattern = phaseMovePatterns[newPhaseIndex];
        pattern?.Execute(this).Forget();
    }
}
