using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Siene_Change_Main_Shooting : MonoBehaviour
{
    // 敵を格納する配列（毎フレーム GameObject.Find で更新される）
    [HideInInspector] public List<GameObject> enemyBox = new List<GameObject>();

    [HideInInspector] public GameObject[] itemBox;



    // シーン切り替え中かどうかのフラグ
    // → 1度切り替え処理が始まったら、二重に呼ばれないようにする
    public bool isChangingScene = false;
    public bool lastEnemyDead = false;
    public bool playerDead = false;

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
    public static Siene_Change_Main_Shooting Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        lastEnemyDead = false;
    }

    void Update()
    {
        // すでにシーン切り替え中なら処理しない
        if (isChangingScene) return;

        //  ここの処理重くない？
        // タグ "Enemy" のついた全オブジェクトを取得
     //   enemyBox = GameObject.FindGameObjectsWithTag("Enemy");


        itemBox = GameObject.FindGameObjectsWithTag("Item");
        // タグ "Player" のついた全オブジェクトを取得
       // player = GameObject.FindGameObjectsWithTag("Player");

        // 敵が全滅した場合
        if (lastEnemyDead == true && itemBox.Length == 0&& enemyBox.Count==0)
        {
            // 2秒後に Shop へ移動
            // 第3引数 true → 「このあと次のステージに進む」ことを示す
            StartCoroutine(ChangeSceneWithDelay("Shop", 2f, true));
        }

      


        // プレイヤーが全滅した場合
        if (playerDead==true)
        {
            // 2秒後に GameOver へ移動
            // 第3引数 false → 「次のステージには進まない（GameOver固定）」
            BulletPool.Instance.ClearPool(); //弾プールクリア
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
 
        if (SceneEffect.Instance!=null) yield return new WaitForSeconds(SceneEffect.Instance.finishFadeSpeed);
     
        // Shopに行く場合（goNextStage が true のときのみ）
        if (goNextStage && sceneName == "Shop")
        {
            // まず Shop シーンをロード
          
            SceneManager.LoadScene(sceneName);

            // 次のステージに進む処理自体は Shop シーン内で
            // 「ボタンを押す」などのイベントで GoToNextStage() を呼び出すのが自然
        }
        else
        {
            // それ以外の場合（GameOverやその他シーン）は即ロード
            SceneManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// Shopシーン内から呼び出して、次のステージに進むための関数
    /// （UIボタンの OnClick に設定する想定）
    /// </summary>
    public async static void GoToNextStage()
    {
        // まだ未消化のステージが残っている場合
        if (currentStageIndex < Instance.stageOrder.Length)
        {
            // 次のステージを取得
            string nextStage = Instance.stageOrder[currentStageIndex];

            // ステージへ移動
            //暗転処理
       if(SceneEffect.Instance!=null)  await    SceneEffect.Instance.FadeOut();
     
            SceneManager.LoadScene(nextStage);

            // インデックスを進める（次呼ばれたときは次のステージ）
            currentStageIndex++;
        }
        else
        {
            // 全てのステージをクリアしたら GameClear シーンへ
         
            SceneManager.LoadScene("GameClear");
        }
    }

    //敵の登録
    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemyBox.Contains(enemy))
            enemyBox.Add(enemy);
    }

    // 削除用
    public void UnregisterEnemy(GameObject enemy)
    {
        enemyBox.Remove(enemy);
    }


}