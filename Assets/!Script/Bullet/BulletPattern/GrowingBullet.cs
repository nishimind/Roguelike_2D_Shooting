using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingBullet : BulletBase
{
    [SerializeField, Header("成長速度")]
    private float _growthRate = 0.5f; // 秒あたりの成長率

    [SerializeField, Header("最大サイズ")]
    private float _maxScale = 3f; // 最大スケール

    protected override void Initialize()
    {
        // 通常より遅くする
        _speed = 2f;
        transform.localScale = Vector3.one; // 初期サイズをリセット
    }

    protected override void Update()
    {
        base.Update(); // 移動は親クラスに任せる

        // 大きくなる処理
        if (transform.localScale.x < _maxScale)
        {
            float growth = _growthRate * Time.deltaTime;
            transform.localScale += new Vector3(growth, growth, 0f);
        }
    }
}