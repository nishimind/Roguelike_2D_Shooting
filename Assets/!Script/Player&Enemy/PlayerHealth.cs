using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class PlayerHealth : HealthBase
{
    private PlayerStatus status;
    private GameManager gameManager;
    private CinemachineImpulseSource shaker;
    private bool isDead = false;

    protected override void Start()
    {
        base.Start();

        // PlayerStatusを参照
        status = PlayerStatus.Instance;
        gameManager = FindObjectOfType<GameManager>();
        shaker = FindObjectOfType<CinemachineImpulseSource>();

        // 初期HPを同期
        status.currentHP = status.maxHP;
    }

    public override void TakeDamage(float damage)
    {
        status.currentHP -= damage;

        if (status.currentHP <= 0 && isDead == false)
        {
            status.currentHP = 0;
            isDead = true;
            Die();
        }
        else
        {
            StartBlink();
        }
    }

    protected override async void Die()
    {
        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        if (shaker != null)
            shaker.GenerateImpulse();

        Debug.Log("Player Dead!");
        gameManager.DeadEffect();
        await UniTask.Delay(2000);
        Destroy(gameObject); // プレイヤーは即削除しない
    }
}
