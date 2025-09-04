using UnityEngine;

public class Camera_Chacker : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Camera _mainCam;
    [SerializeField] public BulletPool _pool;

    // �e�Ƀv�[�����Z�b�g
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

        // Main Camera �ɉf���Ă��Ȃ���΃v�[���ɕԋp
        if (!IsVisibleFrom(_mainCam))
        {
            //Debug.Log("��ʊO");
            _pool.Release(gameObject);
        }
    }

    // �J�����̎����䔻��
    private bool IsVisibleFrom(Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
    }
}
