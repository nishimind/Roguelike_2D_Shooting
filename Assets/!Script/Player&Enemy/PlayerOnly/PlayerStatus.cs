using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("ステータス")]
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

    [Header("ステータスUI表示設定")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] public TextMeshProUGUI damageText;
    [SerializeField] public TextMeshProUGUI actualDamageText;
    [SerializeField] public TextMeshProUGUI speedText;
    [SerializeField] public TextMeshProUGUI shootTimeText;

    [Header("ショット設定")]
    public List<GameObject> availableShots = new List<GameObject>(); // 使用可能なショットのプレハブ
    public Transform shotSpawn; // 弾を発射する位置

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
            Debug.LogWarning("[Awake] Player が見つかりませんでした。処理をスキップします。");

        }
    }

    private void Update()
    {
        health.maxHP = maxHp;
        //Updateで合わせてもいいのか？

        playerMovement.bullletPower = attackPower;
        playerMovement._shootTime = shootTime;
        playerMovement.moveSpeed=speed;

        //UI表示
        hpText.text = "HP: " + health.currentHP + "/" + health.maxHP;
        powerText.text="Power:"+attackPower;
        moneyText.text="Money:"+Money;
        defenceText.text= "Defence:" + defencePower;
        shootTimeText.text="shootTime:"+shootTime;
        speedText.text = "speed:" + speed;

    }
    // 新しいショットを追加する処理
    /*
    public void AddShotType(int typeId)
    {
        // 仮に typeId = 0,1,2 に対応するショットプレハブを追加する例
        if (typeId >= 0 && typeId < availableShots.Count)
        {
            Debug.Log("新しいショットを追加: " + availableShots[typeId].name);
            // ここで実際にショットの切り替えや追加処理を書く
            // 例えばショットリストに追加するだけでもOK
        }
        else
        {
            Debug.LogWarning("AddShotType: typeIdが範囲外です。");
        }
    }*/
 

    
   
}
