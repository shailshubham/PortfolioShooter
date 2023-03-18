using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCharacter;

public class Jump : IState
{
    Animator anim;
    InputData inputData;
    CharacterMover mover;
    AnimationRiggingController rigController;
    PlayerCharacter character;
    bool land = false;

    public Jump(PlayerCharacter character)
    {
        anim = character.Anim;
        mover = character.CharacterMover;
        inputData = character.inputData;
        rigController = character.RigController;
        this.character = character;
    }
    public void OnEnter()
    {
        land = false;
        mover.Jump();
        if (character.StateMachine.previousState == character.run)
            anim.SetTrigger("runJump");
        else
            anim.SetTrigger("jump");
    }

    public void Update()
    {

        if (mover.GroundDistance < .5f&&!land && mover.velocity.y <0)
        {

            land = true;
        }
        //updating characterMovement
        if (character.StateMachine.previousState == character.run)
            mover.MoveCharacter(1f);
        else
            mover.MoveCharacter(.5f);

    }

    public void OnExit()
    {
        anim.SetTrigger("land");
    }
}
