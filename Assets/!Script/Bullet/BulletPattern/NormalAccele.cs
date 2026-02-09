using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AttackPattern/弾の速度を少しずつあげる")]

public class NormalAccele :AttackPatternSO
{
    public float initialSpeed = 5f;
    public float acceleration = 5f;
 public float maxSpeed = 10f;   
    private float _currentSpeed;
    void Start()
    {
        _currentSpeed = initialSpeed;
    }
    public override void Shoot(Vector3 position, int rotation, GameObject bulletPrefab, float damage)
    {


        GameObject bullet = BulletPool.Instance.Get(bulletPrefab, position, Quaternion.Euler(0, 0, rotation));

        // 初速をセット
        _currentSpeed += acceleration * Time.deltaTime;
        if(maxSpeed>=initialSpeed)
        _currentSpeed = Mathf.Min(_currentSpeed, maxSpeed);
        else
            _currentSpeed = Mathf.Max(_currentSpeed, maxSpeed);

        var bulletBase = bullet.GetComponent<BulletBase>();
        bulletBase._speed = _currentSpeed;
        bulletBase.angleDeg = rotation ;
        // 攻撃力をセット
        var bulletDamage = bullet.GetComponent<BulletDamage>();
        bulletDamage.damage = damage;
        if (isPlyerBullet) bulletDamage.damage *= PlayerStatus.Instance.attackPower;

        bullet.transform.rotation = Quaternion.Euler(0, 0, rotation) * Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }
}