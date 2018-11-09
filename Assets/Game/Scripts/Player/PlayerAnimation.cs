using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //handle animator
    private Animator _anim;

    private void Start()
    {
        //assign handle to animator
        _anim = GetComponentInChildren<Animator>();
    }

    public void Move()
    {
        _anim.SetTrigger("Move");
    }

    public void Idle()
    {
        _anim.SetTrigger("Idle");
    }

    //Attack method
    public void AttackRight()
    {
        _anim.SetTrigger("AttackRight");
        //play sword animation
    }

    public void AttackLeft()
    {
        _anim.SetTrigger("AttackLeft");
        //play sword animation
    }
}
