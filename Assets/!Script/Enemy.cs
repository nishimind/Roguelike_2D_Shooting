using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //�ϐ���ݒ�
    [SerializeField,Header("�e�I�u�W�F�N�g")]
    private GameObject _bullet;
    [SerializeField, Header("�e�̔��˂��鎞��")]
    private float _shootTime;

    //�v���C���[��ݒ肷��ϐ�
    private GameObject _player;
    //�e�𔭎˂��鎞�Ԃ��J�E���g����ϐ�
    private float _shootCount;


    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[��T��
        _player = FindObjectOfType<PlayerMovement>().gameObject;
        _shootCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _Shooting();
    }

    private void _Shooting()
    {
        //�e�𔭎˂���
        _shootCount += Time.deltaTime;
        if(_shootCount < _shootTime) return;

        //�e�𐶐�����
        GameObject bulletObj = Instantiate(_bullet);
        //���������e��G�̍��W�ɐݒ肷��
        bulletObj.transform.position = transform.position;
        //�G����v���C���[�Ɍ������Ēe�𔭎˂���i�x�N�g���j
        Vector3 dir = _player.transform.position - transform.position;
        bulletObj.transform.rotation = Quaternion.FromToRotation(transform.up,dir);
        _shootCount = 0.0f;
    }
}
