using System.Collections.Generic;
using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
    public int CurrentPhase { get; private set; }

    [Header("フェーズ移行HP(%)")]
    public List<float> HPlist = new();

    [SerializeField] private Animator animator;
    [SerializeField] private EnemyHealth health;

    void Update()
    {
        UpdatePhase();
    }

    public void UpdatePhase()
    {
        float hpPercent = (health.currentHP / health.maxHP) * 100f;

        if (CurrentPhase < HPlist.Count && hpPercent <= HPlist[CurrentPhase])
        {
            ChangePhase(CurrentPhase + 1);
        }
    }

    void ChangePhase(int newPhase)
    {
        CurrentPhase = newPhase;

        animator.SetInteger("Phase", newPhase);
    }
}