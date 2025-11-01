using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Siene_Change_Main_Shooting : MonoBehaviour
{
    // 敵を格納する配列（毎フレーム GameObject.Find で更新される）
    private GameObject[] enemyBox;
    private GameObject[] itemBox;

    // プレイヤーを格納する配列
    private GameObject[] player;

    // シーン切り替え中かどうかのフラグ
    // → 1度切り替え処理が始まったら、二重に呼ばれないようにする
    public bool isChangingScene = false;
    //フェードアウト関連
    public float fadeSpeed = 0.2f;
    public float alpha = 1f;
    public Image image;
    //ショップかどうかのフラグ
    public bool isShop = false;
    [SerializeField]
    private string[] stageOrder =
 {
    "Stage2", "Stage3",
    "MiddleBoss",   // ← 中ボスステージ
    "Stage4", "Stage5", "Stage6",
    "Boss"       // ← 最終ボスステージ
};

    // 現在のステージ番号を保持する変数
    // static にしているのでシーンを跨いでも値が保持される（アプリ終了までは残る）
    public static int currentStageIndex = 1;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 既に存在するなら自分を破棄
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject); // 永続化
        SceneManager.sceneLoaded += OnSceneLoaded;
        // シーンが切り替わっても参照できるように static 変数に代入

    }
    void Update()
    {
        // すでにシーン切り替え中なら処理しない
        if (isChangingScene ||  isShop) return;

        //  ここの処理重くない？

        // タグ "Enemy" のついた全オブジェクトを取得
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        itemBox = GameObject.FindGameObjectsWithTag("Item");
        // タグ "Player" のついた全オブジェクトを取得
        player = GameObject.FindGameObjectsWithTag("Player");

        // 敵が全滅した場合
        if (enemyBox.Length == 0 && itemBox.Length == 0)
        {
            // 2秒後に Shop へ移動
            // 第3引数 true → 「このあと次のステージに進む」ことを示す
            StartCoroutine(ChangeSceneWithDelay("Shop", 2f, true));
        }

        // プレイヤーが全滅した場合
        if (player.Length == 0)
        {
            // 2秒後に GameOver へ移動
            // 第3引数 false → 「次のステージには進まない（GameOver固定）」
            StartCoroutine(ChangeSceneWithDelay("GameOver", 2f, false));
        }
    }

    /// <summary>
    /// 指定したシーンに一定時間待ってから移動するコルーチン
    /// </summary>
    /// <param name="sceneName">移動先のシーン名</param>
    /// <param name="delay">待機時間</param>
    /// <param name="goNextStage">Shopのあとに次ステージへ進むかどうか</param>
    private IEnumerator ChangeSceneWithDelay(string sceneName, float delay, bool goNextStage)
    {
        // フラグを立てて多重呼び出し防止
        isChangingScene = true;

        // 指定時間待つ
        yield return new WaitForSeconds(delay);

        // Shopに行く場合（goNextStage が true のときのみ）
        if (goNextStage && sceneName == "Shop")
        {
            // まず Shop シーンをロード
            isShop = true;
            SceneManager.LoadScene(sceneName);

            // 次のステージに進む処理自体は Shop シーン内で
            // 「ボタンを押す」などのイベントで GoToNextStage() を呼び出すのが自然
        }
        else
        {
            isShop = false;
            // それ以外の場合（GameOverやその他シーン）は即ロード
            SceneManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// Shopシーン内から呼び出して、次のステージに進むための関数
    /// （UIボタンの OnClick に設定する想定）
    /// </summary>
    public static void GoToNextStage()
    {
        // フェードアウトを開始
     
        Instance?.FadeOut().Forget(); // ← ここで呼べるようにする！

        // まだ未消化のステージが残っている場合
        if (currentStageIndex < Instance.stageOrder.Length)
        {
            // 次のステージを取得
            string nextStage = Instance.stageOrder[currentStageIndex];

            // ステージへ移動
            SceneManager.LoadScene(nextStage);

            // インデックスを進める
            currentStageIndex++;
        }
        else
        {
            // 全てのステージをクリアしたら GameClear シーンへ
            SceneManager.LoadScene("GameClear");
        }
    }



    // シングルトン的に利用するためのインスタンス保持
    public static Siene_Change_Main_Shooting Instance { get; private set; }


    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       isChangingScene = false;
        alpha = 1f;
        Debug.Log("明るくなる");
        while (alpha > 0)
        {//画面を明るくする
            alpha -= Time.deltaTime / fadeSpeed;
            image.color = new Color(0, 0, 0, alpha);
            await UniTask.Yield();
        }
        await UniTask.WaitUntil(() =>isChangingScene==true);
        FadeOut();

    }
    public async UniTask FadeOut()
    {//画面を黒くする
        Debug.Log("フェードアウト開始");
        alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime / fadeSpeed;
            image.color = new Color(0, 0, 0, alpha);
            await UniTask.Yield();
        }
    }

}