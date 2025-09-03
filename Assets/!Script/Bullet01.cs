using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet01 : MonoBehaviour
{
    [SerializeField, Header("�e�̑��x")]
    private float _speed;
    [SerializeField, Header("�e�̈З�")]
    private int _power;

    private Rigidbody2D _rb;  
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();

    }

    private void _Move()
    {
        _rb.velocity = transform.up * _speed;
    }
}
