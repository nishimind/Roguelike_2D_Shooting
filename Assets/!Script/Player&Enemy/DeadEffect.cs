using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEffect : MonoBehaviour
{
    // Animatorコンポーネントをとってくる
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }

    private void Dead()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) //レイヤー番号して
        Destroy(gameObject);
    }
}
