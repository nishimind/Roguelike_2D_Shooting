using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
   
    // 各プレハブごとにプールを分ける
    private readonly Dictionary<GameObject, Queue<GameObject>> poolDictionary = new();
    public static BulletPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // 敵の出現時などに呼び出して、弾プレハブを登録しておく
    public void RegisterBulletPrefab(GameObject bulletPrefab,int poolSize)
    {
        if (poolDictionary.ContainsKey(bulletPrefab))
            return;

        var newPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            var bullet = CreateNewBullet(bulletPrefab);
            newPool.Enqueue(bullet);
        }

        poolDictionary.Add(bulletPrefab, newPool);
    }

    // 弾を生成して非アクティブ化
    private GameObject CreateNewBullet(GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(false);

        // 🔸ここでPrefab情報を明示的にセット！
        var bullet = obj.GetComponentInChildren<BulletDamage>();
        if (bullet != null)
        {
            bullet.originPrefab = prefab;
        }

        if (obj.TryGetComponent(out CameraChecker checker))
            checker.Init(this);

        return obj;
    }

    // 弾を取得する
    public GameObject Get(GameObject bulletPrefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.TryGetValue(bulletPrefab, out var pool))
        {
            // 登録されていなければ新規登録
            RegisterBulletPrefab(bulletPrefab,1);
            pool = poolDictionary[bulletPrefab];
        }

        GameObject obj = pool.Count > 0 ? pool.Dequeue() : CreateNewBullet(bulletPrefab);

        if (obj.TryGetComponent(out BulletDamage damage))
            damage.grazed = false;
        obj.transform.localScale = bulletPrefab.transform.localScale;
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    // 弾を返却する
    public void Release(GameObject bulletPrefab, GameObject obj)
    {
        obj.SetActive(false);

        if (!poolDictionary.ContainsKey(bulletPrefab))
            poolDictionary.Add(bulletPrefab, new Queue<GameObject>());

        poolDictionary[bulletPrefab].Enqueue(obj);
        
    }
    public void ClearPool()
    {
        foreach (var kvp in poolDictionary)
        {
            var pool = kvp.Value;
            while (pool.Count > 0)
            {
                var bullet = pool.Dequeue();
                if (bullet != null)
                    Destroy(bullet);
            }
        }

        poolDictionary.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }
}
