using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
[CreateAssetMenu(menuName = "MovePattern/まっすぐ加速あり")]

public class Straight_Accele : MovePatternSO
{
    [Header("移動角度（度）")]
    [Tooltip("0=右 / 90=上 / -90=下")]
    public float angleDeg = -90f;

    [Header("初速")]
    public float startSpeed = 0f;

    [Header("加速度（毎秒）")]
    public float acceleration = 5f;

    [Header("最高速")]
    public float maxSpeed = 8f;

    public override async UniTaskVoid Execute(EnemyMovementController controller)
    {
        if (controller == null || controller._rb == null) return;

        float currentSpeed = startSpeed;

        // 角度 → 方向ベクトル
        Vector2 direction = Quaternion.Euler(0f, 0f, angleDeg) * Vector2.right;

        while (controller != null)
        {
            // 加速
            currentSpeed += acceleration*acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            controller._rb.velocity = direction * currentSpeed;

            await UniTask.Yield();
        }
    }
}