using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }


    [Header("ステータス")]
    public int maxHP = 100;
public int currentHP = 100;
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
    public ItemCollector collector;

    [Header("アイテム取得時の文字演出")]
    [SerializeField] private float scaleUpAmount = 1.3f;   // 拡大倍率
    [SerializeField] private float duration = 0.2f;        // 拡大・縮小にかける時間

    private Vector3 originalScale;
    [Header("オプション関係")]
    public bool option1;
    public GameObject option1prefab;
    public void Awake()
    {
        // イベントにイベントハンドラーを追加
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 既に存在するなら自分を破棄
            return;
        }
        if (moneyText != null)
            originalScale = moneyText.rectTransform.localScale;

        Instance = this;
        DontDestroyOnLoad(gameObject); // 永続化

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
        //オプション
        GenerateOption();
    }
    private void Update()
    {
      

        playerMovement.bullletPower = attackPower;
        playerMovement._shootTime = shootTime;
        playerMovement.moveSpeed=speed;

        //UI表示
        hpText.text = "HP: " +currentHP + "/" + maxHP;
        powerText.text="Power:"+attackPower;
        moneyText.text= Money.ToString();
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
    public void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            health = player.GetComponent<PlayerHealth>();
            playerMovement = player.GetComponent<PlayerMovement>();
         collector = player.GetComponentInChildren<ItemCollector>();

        
            playerMovement.bullletPower = attackPower;
            playerMovement._shootTime = shootTime;
        }
        else
        {
            Debug.LogWarning("[Awake] Player が見つかりませんでした。処理をスキップします。");

        }
    }

    public void UpdateMoneyText()
    {
      

        // 既にアニメーション中なら一旦リセット
        moneyText.rectTransform.DOKill();

        // DOTweenで「拡大 → 戻る」アニメーション
        moneyText.rectTransform
            .DOScale(originalScale * scaleUpAmount, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                moneyText.rectTransform
                    .DOScale(originalScale, duration)
                    .SetEase(Ease.InQuad);
            });
    }
public void GenerateOption()
    {
      if(option1)  Instantiate(option1prefab, player.transform.position+new Vector3(0.5f,0,0), Quaternion.identity, player.transform);

    }
}
