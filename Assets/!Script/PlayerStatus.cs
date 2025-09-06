using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("�X�e�[�^�X")]
    public int maxHp = 100;
    public int attackPower = 10;
    public int Money = 200;
    [Header("�X�e�[�^�X�\���ݒ�")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [Header("�V���b�g�ݒ�")]
    public List<GameObject> availableShots = new List<GameObject>(); // �g�p�\�ȃV���b�g�̃v���n�u
    public Transform shotSpawn; // �e�𔭎˂���ʒu

    public GameObject player;
    public PlayerHealth health;
    public PlayerMovement playerMovement;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            health = player.GetComponent<PlayerHealth>();
            playerMovement = player.GetComponent<PlayerMovement>();

                health.maxHP = maxHp;
           
                playerMovement.bullletPower = attackPower;
            
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

        hpText.text = "HP: " + health.currentHP + "/" + health.maxHP;
        powerText.text="Power:"+attackPower;
        moneyText.text="Money:"+Money;

    }
    // �V�����V���b�g��ǉ����鏈��
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
    }

 

    
   
}
