using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyC : Enemy
{
    private Queue<GameObject> pool = new Queue<GameObject>();
   
    [SerializeField, Header("ˆÚ“®”ÍˆÍ")]
    private float _limitPosY;
    [SerializeField, Header("’ÊíUŒ‚‰ñ”")]
    private int _normalAttackCount;
    [SerializeField, Header("î’e‚Ì’e”")]
    private int _ougiBulletNum;
    [SerializeField, Header("î’e‚ÌL‚ª‚èŠp“x")]
    private float _ougiAngle;
    [SerializeField, Header("î’e‚ÌUŒ‚‰ñ”")]
    private int _ougiAttackCount;
    enum AttackMode
    {
        Normal,
        Ougi,
        leftright,
    }

    private int _currentNormalAttackCount;
    private AttackMode _attackMode;

    protected override void _Initialize()
    {
        _currentNormalAttackCount = 0;
        _attackMode = AttackMode.Normal;
    }

    protected override void _Move()
    {
        if(transform.position.y <= _limitPosY)
        {
            _rb.velocity =  Vector2.zero;
            _bAttack = true;
            return;
        }
       
        base._Move();
        _bAttack = false;
    }
    //UŒ‚‚ğ2í—Ş‚É•ª‚¯‚é
    protected override void _Attack()
    {
        switch(_attackMode)
        {
            case AttackMode.Normal:
                _NoramalShooting();
                break;
            case AttackMode.Ougi:
                //‰œ‹`UŒ‚
                break;
        }
    }
    private void _NoramalShooting()
    {
        //’e‚ğ”­Ë‚·‚é
        _shootCount += Time.deltaTime;
        if (_shootCount < _shootTime) return;

        //’e‚ğ¶¬‚·‚é
        //  GameObject bulletObj = Instantiate(_bullet);
        GameObject bulletObj = _bulletPooler.Get(transform.position, transform.rotation);

        //¶¬‚µ‚½’e‚ğ“G‚ÌÀ•W‚Éİ’è‚·‚é
        bulletObj.transform.position = transform.position;       
        bulletObj.transform.rotation = Quaternion.FromToRotation(transform.up, Vector2.down);
       
        _shootCount = 0;
        //’ÊíUŒ‚‰ñ”‚ğƒJƒEƒ“ƒg 
        _currentNormalAttackCount++;
        //ˆê’è‰ñ”‚Å‰œ‹`UŒ‚‚ÉØ‚è‘Ö‚¦
        if (_currentNormalAttackCount >= _normalAttackCount)
        {
            _attackMode = AttackMode.Ougi;
            _currentNormalAttackCount = 0;
        }
    }

   
    
}
