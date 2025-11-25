using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class MovePatternSO : ScriptableObject
{
    public abstract UniTaskVoid Execute(EnemyMovementController controller);
}
