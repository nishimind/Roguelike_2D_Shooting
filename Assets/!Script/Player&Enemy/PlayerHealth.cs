using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("(PlayerStatusから設定)")]
    public int maxHP = 10;
    public int currentHP;
    public bool isPlayer;

    public PlayerStatus status;

    [SerializeField, Header("死亡時effect")]
    private GameObject deadEffect;
    private GameManager gameManager;
    private Cinemachine.CinemachineImpulseSource shaker;

    [Header("ダメージ演出")]
    [SerializeField, Header("点滅時間(秒)")]
    private float damageTime = 0.5f;
    [SerializeField, Header("点滅周期(秒)")]
    private float damageCycle = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float damageTimeCount;
    private bool isDamage;
    private EnemyDropper enemyDropper;
    void Start()
    { enemyDropper = gameObject.GetComponent<EnemyDropper>();
        currentHP = maxHP;

        if (isPlayer)
            status = GameObject.FindWithTag("PlayerStatus").GetComponent<PlayerStatus>();

        gameManager = FindObjectOfType<GameManager>();
        shaker = FindObjectOfType<Cinemachine.CinemachineImpulseSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        damageTimeCount = 0;
        isDamage = false;
    }

    void Update()
    {
        DamageBlink();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
        else
        {
            // 点滅開始
            isDamage = true;
            damageTimeCount = 0;
        }
    }

    void Die()
    {
        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        if (shaker != null&&isPlayer)
            shaker.GenerateImpulse();

        if (isPlayer)
        {
            Debug.Log("Player Dead!");
            gameManager.DeadEffect();
            // Destroy(gameObject); ← プレイヤーはすぐ消さない
        }
        else
        {
            enemyDropper?.DropItems();
            Destroy(gameObject);
        }
    }

    // HPを回復する処理
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log("HP回復: " + amount + " 現在HP: " + currentHP);
    }

  

    // ダメージ点滅処理
    private void DamageBlink()
    {
        if (!isDamage) return;

        damageTimeCount += Time.deltaTime; // 経過時間を加算

        float value = Mathf.Repeat(damageTimeCount, damageCycle);
        spriteRenderer.enabled = value >= damageCycle * 0.5f;

        if (damageTimeCount >= damageTime)
        {
            damageTimeCount = 0;
            spriteRenderer.enabled = true; // 最後は表示状態に戻す
            isDamage = false;
        }
    }
}


