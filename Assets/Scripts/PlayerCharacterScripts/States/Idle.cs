using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCharacter;

public class Idle : IState
{
    Animator anim;
    CharacterController controller;
    CharacterData characterData;
    public Idle(Animator animator, CharacterController controller,CharacterData characterData)
    {
        anim= animator;
        this.controller = controller;
        this.characterData = characterData;
    }

    public void Update()
    {
        Gravity();
    }
    public void OnEnter() 
    {
        anim.SetFloat("forward", 0f);
        anim.SetFloat("strafe", 0f);
    }

    public void OnExit()
    { 

    }
    void Gravity()
    {
        controller.Move(Vector3.up * characterData.gravity * Time.deltaTime);
    }
}
