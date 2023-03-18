using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCharacter;

public class Idle : IState
{
    Animator anim;
    InputData inputData;
    CharacterMover mover;
    AnimationRiggingController rigController;
    public Idle(PlayerCharacter character)
    {
        anim = character.Anim;
        mover = character.CharacterMover;
        inputData = character.inputData;
        rigController = character.RigController;
    }

    public void Update()
    {
        
    }
    public void OnEnter() 
    {
        anim.SetFloat("forward", 0f);
        anim.SetFloat("strafe", 0f);
        rigController.leftHandWeight = 1f;
    }

    public void OnExit()
    { 

    }
}
