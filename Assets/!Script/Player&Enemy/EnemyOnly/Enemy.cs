using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //�ϐ���ݒ�
    // [SerializeField,Header("�e�I�u�W�F�N�g")]
    //   private GameObject _bullet;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField, Header("�e�̔��˂��鎞��")]
    private float _shootTime;
    [SerializeField, Header("�e�̃v�[���[")]
    
    private BulletPool _bulletPooler;
    [SerializeField, Header("�ړ����x")]
    private float _moveSpeed;

    //�v���C���[��ݒ肷��ϐ�
    private GameObject _player;
    private Rigidbody2D _rb;
    //�e�𔭎˂��鎞�Ԃ��J�E���g����ϐ�
    private float _shootCount;
    //��ʓ��ōU������
    private bool _bAttack;

    // Start is called before the first frame update
    void Start()
    { 
        //�v���C���[��T�� �v���C���[���Ō��~
        if(FindAnyObjectByType<PlayerMovement>() )
        {
           _player = FindObjectOfType<PlayerMovement>().gameObject;
        }      
        _shootCount = 0;
        _bAttack = false;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Shooting();
        _Move();
    }

    private void _Shooting()
    {
        //�e�𔭎˂���
        _shootCount += Time.deltaTime;
        if(_shootCount < _shootTime) return;

        //�e�𐶐�����
        //  GameObject bulletObj = Instantiate(_bullet);
        GameObject bulletObj = _bulletPooler.Get(transform.position, transform.rotation);

        //���������e��G�̍��W�ɐݒ肷��
        bulletObj.transform.position = transform.position;
        //�G����v���C���[�Ɍ������Ēe�𔭎˂���i�x�N�g���j
        Vector3 dir = _player.transform.position - transform.position;
        bulletObj.transform.rotation = Quaternion.FromToRotation(transform.up,dir);
        _shootCount = 0.0f;
    }

    //�������Ɉړ�
    private void _Move()
    {
        _rb.velocity = Vector2.down * _moveSpeed;
    }

    //�J�����Ɏʂ��Ă���ԍU������
    private void OnWillRenderObject()
    {
        if(Camera.current.name == "Main Camera")
        {
            _bAttack = true;
        }
    }
}
