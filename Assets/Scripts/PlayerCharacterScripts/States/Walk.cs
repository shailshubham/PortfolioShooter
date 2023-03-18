using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : IState
{
    Animator anim;
    InputData inputData;
    CharacterMover mover;
    AnimationRiggingController rigController;
    public Walk(PlayerCharacter character)
    {
        anim = character.Anim;
        mover = character.CharacterMover;
        inputData = character.inputData;
        rigController = character.RigController;
    }

        public void Update()
    {
        mover.MoveCharacter(.33f);
        anim.SetFloat("forward", inputData.dpadInput.y);
        anim.SetFloat("strafe", inputData.dpadInput.x);
    }
    public void OnEnter()
    {
        rigController.leftHandWeight = 1f;
    }

    public void OnExit()
    {
        anim.SetFloat("forward", 0f);
        anim.SetFloat("strafe", 0f);
    }
}
