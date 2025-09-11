using UnityEngine;

public class Camera_Chacker : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Camera _mainCam;
    [SerializeField] public BulletPool _pool;

    // 弾にプールをセット
    public void Init(BulletPool pool)
    {
        _pool = pool;
    }

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _mainCam = Camera.main;
    }

    void Update()
    {
        if (_mainCam == null || _renderer == null || _pool == null) return;

        // Main Camera に映っていなければプールに返却
        if (!IsVisibleFrom(_mainCam))
        {
            //Debug.Log("画面外");
            _pool.Release(gameObject);
        }
    }

    // カメラの視錐台判定
    private bool IsVisibleFrom(Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
    }
}
