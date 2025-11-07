using UnityEngine;

public class BulletDamage : MonoBehaviour
{
   [HideInInspector] public int damage = 1; // 弾の威力
    public bool destroyOnHit = true; // 当たったら消えるかどうか
    public bool grazed=false;
    public bool onlyGraze = false;
    [Header("連続ヒット間隔（レーザー用）")]
    public float damageInterval = 0.2f;
    public GameObject originPrefab; // ← これで元のPrefabを記憶

}