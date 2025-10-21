using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var bullet = CreateNewBullet();
            pool.Enqueue(bullet);
        }
    }

    private GameObject CreateNewBullet()
    {
        var obj = Instantiate(bulletPrefab, transform);
        obj.SetActive(false);

        // CameraChecker ‚ð‰Šú‰»
        if (obj.TryGetComponent(out CameraChecker checker))
            checker.Init(this);

        return obj;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : CreateNewBullet();
        obj.GetComponent<BulletDamage>().grazed = false;
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
