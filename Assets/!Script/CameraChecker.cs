using UnityEngine;

public class CameraChecker : MonoBehaviour
{
    [SerializeField] private Camera _mainCam;
    [SerializeField] public BulletPool _pool;

    [Header("画面外判定の余裕（バッファ）")]
    [SerializeField] private float offScreenBuffer = 0.5f;

    void Awake()
    {
        if (_mainCam == null)
            _mainCam = Camera.main;
    }

    void Update()
    {
        if (_mainCam == null || _pool == null) return;

        Vector3 vp = _mainCam.WorldToViewportPoint(transform.position);

        if (vp.x < -offScreenBuffer || vp.x > 1 + offScreenBuffer ||
            vp.y < -offScreenBuffer || vp.y > 1 + offScreenBuffer)
        {
            _pool.Release(gameObject.GetComponent<BulletDamage>().originPrefab, gameObject);
           
        }
    }

    // 弾にプールをセット
    public void Init(BulletPool pool)
    {
        _pool = pool;
    }
}
