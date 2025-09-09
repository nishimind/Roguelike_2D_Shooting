using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{ 
    
    // Start is called before the first frame update
    [Header("(PlayerStatusから設定)")]
        public int maxHP = 10;
        public int currentHP;
    public bool isPlayer;

    public PlayerStatus status;

    [SerializeField,Header("死亡時effect")]
    private GameObject deadEffect;
    private GameManager gameManager;
    /*
    //ダメージを受けた際点滅する
    [SerializeField, Header("点滅時間")]
    private float damageTime;
    [SerializeField, Header("点滅周期")]
    private float damageCyvle;

    private SpriteRenderer spriteRenderer;
    private float damageTimeCount;
    private bool isDamage;
    */

  
        void Start()
        {
            currentHP = maxHP;
        status = GameObject.FindWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        gameManager = FindObjectOfType<GameManager>();
        /*
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageTimeCount = 0;
        isDamage = false;
        */

    }
    　　
        /*
        void Update()
        {
        Damage();
        }
        */
        public void TakeDamage(int damage)
        {
        int realDamage = Mathf.Max(0, damage - status.defencePower);
        status.damageText.text ="damage:"+damage;
        status.actualDamageText.text= "ActualDamage:"+damage+"-"+status.defencePower+"="+realDamage;
        currentHP -= realDamage;
            if (currentHP <= 0)
            {
                currentHP = 0; 
                Die();
               Instantiate(deadEffect, transform.position, Quaternion.identity);
               gameManager.DeadEffect();

        }
        }
    /*
    //ダメージを受けた際の点滅処理
    private void Damage()
    {
        if (!isDamage) return;
       
        damageTimeCount += Time.deltaTime; //点滅時間のカウント

        float value = Mathf.Repeat(damageTimeCount, damageCyvle);　//点滅周期で繰り返す
        spriteRenderer.enabled = value >= damageCyvle * 0.5f;

        if (damageTimeCount >= damageTime)
        {
            damageTimeCount = 0;
            spriteRenderer.enabled = true;
            isDamage = false;
        }
    }
    */

    // HPを回復する処理
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log("HP回復: " + amount + " 現在HP: " + currentHP);
    }

    void Die()
        {
        // ゲームオーバー処理など
        if (isPlayer)
        {
            Debug.Log("Player Dead!");
        }
        else {
            //敵の処理
            Destroy(gameObject); 
        }
        }
    
}
