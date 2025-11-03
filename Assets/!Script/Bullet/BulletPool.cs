using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private int poolSize = 20;

    // ŠeƒvƒŒƒnƒu‚²‚Æ‚Éƒv[ƒ‹‚ğ•ª‚¯‚é
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
    // “G‚ÌoŒ»‚È‚Ç‚ÉŒÄ‚Ño‚µ‚ÄA’eƒvƒŒƒnƒu‚ğ“o˜^‚µ‚Ä‚¨‚­
    public void RegisterBulletPrefab(GameObject bulletPrefab)
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

    // ’e‚ğ¶¬‚µ‚Ä”ñƒAƒNƒeƒBƒu‰»
    private GameObject CreateNewBullet(GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(false);

        if (obj.TryGetComponent(out CameraChecker checker))
            checker.Init(this);

        return obj;
    }

    // ’e‚ğæ“¾‚·‚é
    public GameObject Get(GameObject bulletPrefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.TryGetValue(bulletPrefab, out var pool))
        {
            // “o˜^‚³‚ê‚Ä‚¢‚È‚¯‚ê‚ÎV‹K“o˜^
            RegisterBulletPrefab(bulletPrefab);
            pool = poolDictionary[bulletPrefab];
        }

        GameObject obj = pool.Count > 0 ? pool.Dequeue() : CreateNewBullet(bulletPrefab);

        if (obj.TryGetComponent(out BulletDamage damage))
            damage.grazed = false;

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    // ’e‚ğ•Ô‹p‚·‚é
    public void Release(GameObject bulletPrefab, GameObject obj)
    {
        obj.SetActive(false);

        if (!poolDictionary.ContainsKey(bulletPrefab))
            poolDictionary.Add(bulletPrefab, new Queue<GameObject>());

        poolDictionary[bulletPrefab].Enqueue(obj);
    }
}
