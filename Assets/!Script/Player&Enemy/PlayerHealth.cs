using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{ 
    
    // Start is called before the first frame update
    [Header("(PlayerStatus����ݒ�)")]
        public int maxHP = 10;
        public int currentHP;
    public bool isPlayer;

    public PlayerStatus status;

    [SerializeField,Header("���S��effect")]
    private GameObject deadEffect;
    private GameManager gameManager;
    /*
    //�_���[�W���󂯂��ۓ_�ł���
    [SerializeField, Header("�_�Ŏ���")]
    private float damageTime;
    [SerializeField, Header("�_�Ŏ���")]
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
    �@�@
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
    //�_���[�W���󂯂��ۂ̓_�ŏ���
    private void Damage()
    {
        if (!isDamage) return;
       
        damageTimeCount += Time.deltaTime; //�_�Ŏ��Ԃ̃J�E���g

        float value = Mathf.Repeat(damageTimeCount, damageCyvle);�@//�_�Ŏ����ŌJ��Ԃ�
        spriteRenderer.enabled = value >= damageCyvle * 0.5f;

        if (damageTimeCount >= damageTime)
        {
            damageTimeCount = 0;
            spriteRenderer.enabled = true;
            isDamage = false;
        }
    }
    */

    // HP���񕜂��鏈��
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log("HP��: " + amount + " ����HP: " + currentHP);
    }

    void Die()
        {
        // �Q�[���I�[�o�[�����Ȃ�
        if (isPlayer)
        {
            Debug.Log("Player Dead!");
        }
        else {
            //�G�̏���
            Destroy(gameObject); 
        }
        }
    
}
