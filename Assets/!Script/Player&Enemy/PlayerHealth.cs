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

    // 点滅用
    [SerializeField, Header("点滅時間")]
    private float damageTime = 0.5f;
    [SerializeField, Header("点滅周期")]
    private float damageCycle = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float damageTimeCount;
    private bool isDamage;

    void Start()
    {
        currentHP = maxHP;
        if (isPlayer) status = GameObject.FindWithTag("PlayerStatus").GetComponent<PlayerStatus>();

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
            // ダメージ点滅開始
            isDamage = true;
            damageTimeCount = 0;
        }
    }

    private void Die()
    {
        if (deadEffect != null)
            Instantiate(deadEffect, transform.position, Quaternion.identity);

        if (isPlayer)
        {
            Debug.Log("Player Dead!");
            gameManager.DeadEffect();

            if (shaker != null)
                shaker.GenerateImpulse();  // ← プレイヤー死亡時だけ揺らす
        }
        else
        {
            Destroy(gameObject); // 敵は普通に消えるだけ
        }
    }

    // ダメージ時の点滅処理
    private void DamageBlink()
    {
        if (!isDamage) return;

        damageTimeCount += Time.deltaTime;

        float value = Mathf.Repeat(damageTimeCount, damageCycle);
        spriteRenderer.enabled = value >= damageCycle * 0.5f;

        if (damageTimeCount >= damageTime)
        {
            damageTimeCount = 0;
            spriteRenderer.enabled = true;
            isDamage = false;
        }
    }

    // HPを回復する処理
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log("HP回復: " + amount + " 現在HP: " + currentHP);
    }
}

