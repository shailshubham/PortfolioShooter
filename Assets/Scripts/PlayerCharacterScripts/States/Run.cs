using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : IState
{
    Animator anim;
    InputData inputData;
    CharacterMover mover;
    AnimationRiggingController rigController;
    public Run(PlayerCharacter character)
    {
        anim = character.Anim;
        mover = character.CharacterMover;
        inputData = character.inputData;
        rigController = character.RigController;
    }

    public void Update()
    {
        float x = 1f;
        if(inputData.dpadInput.y<0)
        {
            x = .33f;
        }
        mover.MoveCharacter(x);
        anim.SetFloat("forward", inputData.dpadInput.y);
        anim.SetFloat("strafe", inputData.dpadInput.x);
    }
    public void OnEnter()
    {
        anim.SetBool("run", true);
        rigController.leftHandWeight = 0f;
    }

    public void OnExit()
    {
        anim.SetBool("run", false);
        anim.SetFloat("forward",0f);
        anim.SetFloat("strafe", 0f);
    }
}
