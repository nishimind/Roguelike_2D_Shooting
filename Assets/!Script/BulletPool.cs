using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // あらかじめ生成して非アクティブにしておく
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
            Camera_Chacker bulletScript = obj.GetComponent<Camera_Chacker>();
            if (bulletScript != null)
            {
                bulletScript.Init(this);
            }

        }
    }

    // 弾を取得
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (pool.Count > 0 && !pool.Peek().activeInHierarchy)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(bulletPrefab); // 足りなければ追加生成
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        pool.Enqueue(obj);
        return obj;
    }
    public void Release(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}
