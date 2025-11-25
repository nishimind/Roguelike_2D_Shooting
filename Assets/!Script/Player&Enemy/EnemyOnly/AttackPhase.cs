using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackPhase
{
    [Header("このフェーズで実行する攻撃セット")]
    public List<AttackSet> attackSets = new();

    [Header("フェーズ終了条件（どちらも有効）")]
    public float durationSeconds = 0f; // 0以下なら時間条件なし
    public float nextPhaseHpPercent = -1f; // -1ならHP条件なし

    [HideInInspector] public float timer = 0f;
}
