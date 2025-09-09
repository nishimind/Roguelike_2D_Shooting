using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEffect : MonoBehaviour
{
    // Animator�R���|�[�l���g���Ƃ��Ă���
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
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) //���C���[�ԍ�����
        Destroy(gameObject);
    }
}
