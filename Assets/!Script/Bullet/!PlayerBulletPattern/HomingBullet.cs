using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
public class HomingBullet : BulletBase
{
   
    [SerializeField] private float _rotateSpeed = 200f;
    [SerializeField] private float _maxRotateAngle = 30f; // 最大回転角度（度）
   
    private Transform _target;



    protected override void Initialize()
    {
        base.Initialize();
    
    
        // 発射時点で最も近いEnemyタグのターゲットを探す
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            float minDist = Mathf.Infinity;
            foreach (var e in enemies)
            {
                float dist = Vector2.Distance(transform.position, e.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    _target = e.transform;
                }
            }
        }
     /*  await UniTask.Delay(3000).ContinueWith(() =>
        {
            // 10秒後に弾を消す
            _target = null;
        });*/
    }
    protected virtual void Update()
    {

    }
    void FixedUpdate()
    {
        if (_rb == null) return;

        if (_target == null)
        {
            // ターゲットがいなければ直進
            _rb.velocity = transform.up * _speed;
            return;
        }


        // 前進
        _rb.velocity = transform.up * _speed;
        Vector2 dir = (_target.position - transform.position).normalized;
        _rb.AddForce(dir * _rotateSpeed * Time.fixedDeltaTime);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, target,
        _rotateSpeed * Time.fixedDeltaTime);
    }
}
