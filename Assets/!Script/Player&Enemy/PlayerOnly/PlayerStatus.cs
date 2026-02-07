using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using static Enemy;


public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }


    [Header("ステータス")]
    public float maxHP = 100;
public float currentHP = 100;
    public int Money = 200;
    public int attackPower = 10;
    public int defencePower = 0;
    public float speed = 5;
    private bool isSlowed = false;
    public float shootTime = 0.5f;
    public int grazeCount=0;
    public int score = 0;
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
    [SerializeField] public TextMeshProUGUI grazeCountText;
    [SerializeField] public TextMeshProUGUI scoreText;


    [Header("ショット設定")]

    public List<ShotType> availableShots = new List<ShotType>(); // 使用可能なショットのプレハブ


    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerHealth health;
   [HideInInspector] public PlayerMovement playerMovement=PlayerMovement.Instance;
    [HideInInspector] public int x;
    public Dictionary<ItemType, bool> itemFlags = new Dictionary<ItemType, bool>();
    public ItemCollector collector;

    [Header("アイテム取得時の文字演出")]
    [SerializeField] private float scaleUpAmount = 1.3f;   // 拡大倍率
    [SerializeField] private float duration = 0.2f;        // 拡大・縮小にかける時間

    private Vector3 originalScale;
    [Header("オプション関係")]
  
  //  [SerializeField] public OptionTable optionTable; // 設定ファイル参照
    [SerializeField] private float radius = 2f;       // プレイヤーからの距離
    [SerializeField] private float spacing = 1.5f;    // 横並びの間隔
    public OptionData[] optionTable; // オプションの一覧


    public void Awake()
    {
     
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 既に存在するなら自分を破棄
            return;
        }
        if (moneyText != null)
            originalScale = moneyText.rectTransform.localScale;
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // 永続化
        currentHP=maxHP;

     
        // イベントにイベントハンドラーを追加
        SceneManager.sceneLoaded -= OnSceneLoaded; // 念のため重複を解除
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
   
    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[PlayerStatus] OnSceneLoaded called in scene: {scene.name}");
        await UniTask.WaitUntil(() => GameObject.FindWithTag("Player") != null);

        FindPlayer();
        //オプション
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        //GenerateOption();
        //弾のPool登録
        foreach (var set in availableShots)
        {
            set.shootCount = 0;

            if (set.bulletPrefab != null)
                BulletPool.Instance.RegisterBulletPrefab(set.bulletPrefab, set.poolSize);
        }
    }

    private void Update()
    {
      

      
      

        //UI表示
        hpText.text = "HP: " +currentHP + "/" + maxHP;
        powerText.text="Power:"+attackPower;
        moneyText.text= Money.ToString();
        defenceText.text= "Defence:" + defencePower;
        shootTimeText.text="shootTime:"+shootTime;
        speedText.text = "speed:" + speed;
        grazeCountText.text="graze:"+grazeCount;
        scoreText.text = "score:" + score;

    }
   
    public void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            health = player.GetComponent<PlayerHealth>();
            playerMovement = player.GetComponent<PlayerMovement>();
         collector = player.GetComponentInChildren<ItemCollector>();

        
        
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

    

    public void ApplySlow(float amount, float duration)
    {
        Debug.Log("スロー呼ばれたぜ");
        if (isSlowed) return; // 二重適用防止

        isSlowed = true;
        float originalSpeed = speed;
        speed *= amount; // スピードを低下

        // 一定時間後に戻す（UniTask使用）
        RestoreSpeed(duration, originalSpeed).Forget();
    }

    private async UniTaskVoid RestoreSpeed(float duration, float originalSpeed)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(duration));
        speed = originalSpeed;
        isSlowed = false;
    }

  /*  private void SpawnOption(GameObject prefab, Vector3 offset)
    {
        GameObject opt = Instantiate(prefab, player.transform);
        opt.transform.localPosition = offset;

        // ここで Manager に登録
        player.GetComponent<OptionManager>().options.Add(opt.transform);
    }*/


    // 円形配置
    private async void GenerateCircle(OptionData option)
    {
        int count = option.count;
        for (int i = 0; i < count; i++)
        {await UniTask.Delay(TimeSpan.FromSeconds(0.2f)); 
            // 角度を均等に割り振る
            float angle = (i) * Mathf.PI * 2f / count;

            // x,z座標計算（Yはプレイヤーと同じ高さ）a
            Vector3 offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * radius;

            GameObject opt = Instantiate(option.optionPrefab, player.transform.position + offset, Quaternion.identity);

            // プレイヤーを中心に向ける場合
            // opt.transform.LookAt(player.position);

            // プレイヤーの子にして追従
            opt.transform.SetParent(player.transform);
        }
    }

    // 横並び配置
    private void GenerateLine(OptionData option)
    {
        int count = option.count;
        float startX = -(count - 1) * spacing / 2f; // 中心揃え
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(startX + i * spacing, 0f, 0f);
            GameObject opt = Instantiate(option.optionPrefab, player.transform.position + offset, Quaternion.identity);
            opt.transform.SetParent(player.transform);
        }
    }
}