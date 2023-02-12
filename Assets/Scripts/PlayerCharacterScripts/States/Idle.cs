using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    Animator anim;
    public Idle(Animator animator)
    {
        anim= animator;
    }

    public void Update()
    {

    }
    void OnEnter() 
    {
        anim.SetFloat("forward", 0f);
        anim.SetFloat("Strafe", 0f);
    }

    void OnExit()
    { 

    }
}
