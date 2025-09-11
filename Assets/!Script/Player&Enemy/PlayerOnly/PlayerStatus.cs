using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("�X�e�[�^�X")]
    public int maxHp = 100;
    public int Money = 200;
    public int attackPower = 10;
    public int defencePower = 0;
    public float speed = 5;
    public float shootTime = 0.5f;

    public enum ItemType
    {
        Key,
        Sword,
        Shield,
        Potion
    }

    [Header("�X�e�[�^�XUI�\���ݒ�")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] public TextMeshProUGUI damageText;
    [SerializeField] public TextMeshProUGUI actualDamageText;
    [SerializeField] public TextMeshProUGUI speedText;
    [SerializeField] public TextMeshProUGUI shootTimeText;

    [Header("�V���b�g�ݒ�")]
    public List<GameObject> availableShots = new List<GameObject>(); // �g�p�\�ȃV���b�g�̃v���n�u
    public Transform shotSpawn; // �e�𔭎˂���ʒu

    public GameObject player;
    public PlayerHealth health;
    public PlayerMovement playerMovement;
    public Dictionary<ItemType, bool> itemFlags = new Dictionary<ItemType, bool>();

    public void Awake()
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            health = player.GetComponent<PlayerHealth>();
            playerMovement = player.GetComponent<PlayerMovement>();

                health.maxHP = maxHp;
           
                playerMovement.bullletPower = attackPower;
            playerMovement._shootTime = shootTime;
        }
        else
        {
            Debug.LogWarning("[Awake] Player ��������܂���ł����B�������X�L�b�v���܂��B");

        }
    }

    private void Update()
    {
        health.maxHP = maxHp;
        //Update�ō��킹�Ă������̂��H

        playerMovement.bullletPower = attackPower;
        playerMovement._shootTime = shootTime;
        playerMovement.moveSpeed=speed;

        //UI�\��
        hpText.text = "HP: " + health.currentHP + "/" + health.maxHP;
        powerText.text="Power:"+attackPower;
        moneyText.text="Money:"+Money;
        defenceText.text= "Defence:" + defencePower;
        shootTimeText.text="shootTime:"+shootTime;
        speedText.text = "speed:" + speed;

    }
    // �V�����V���b�g��ǉ����鏈��
    /*
    public void AddShotType(int typeId)
    {
        // ���� typeId = 0,1,2 �ɑΉ�����V���b�g�v���n�u��ǉ������
        if (typeId >= 0 && typeId < availableShots.Count)
        {
            Debug.Log("�V�����V���b�g��ǉ�: " + availableShots[typeId].name);
            // �����Ŏ��ۂɃV���b�g�̐؂�ւ���ǉ�����������
            // �Ⴆ�΃V���b�g���X�g�ɒǉ����邾���ł�OK
        }
        else
        {
            Debug.LogWarning("AddShotType: typeId���͈͊O�ł��B");
        }
    }*/
 

    
   
}
