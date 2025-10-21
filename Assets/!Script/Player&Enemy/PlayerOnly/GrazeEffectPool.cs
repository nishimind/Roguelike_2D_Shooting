using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GrazeEffectPool : MonoBehaviour
{
    public static GrazeEffectPool Instance;

    [Header("プールするエフェクトPrefab")]
    public GameObject grazeEffectPrefab;

    [Header("プール数")]
    public int poolSize = 50;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(grazeEffectPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetEffect(Vector3 position)
    {
        if (pool.Count == 0)
        {
            // 必要なら動的に追加
            GameObject newObj = Instantiate(grazeEffectPrefab, transform);
            newObj.SetActive(false);
            pool.Enqueue(newObj);
        }

        GameObject effect = pool.Dequeue();
        effect.transform.position = position;
        effect.SetActive(true);

        // 一定時間後に戻す
        StartCoroutine(ReturnToPool(effect, 1f));
        return effect;
    }

    private IEnumerator ReturnToPool(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
