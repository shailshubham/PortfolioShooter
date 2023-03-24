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
    WeaponSystem weaponSystem;
    CameraController camController;
    bool land = false;

    public Jump(PlayerCharacter character)
    {
        anim = character.Anim;
        mover = character.CharacterMover;
        inputData = character.inputData;
        rigController = character.RigController;
        this.character = character;
        weaponSystem = character.weaponSystem;
        camController = character.camController;
    }
    public void OnEnter()
    {
        camController.RunAim();
        rigController.leftHandWeight = 0f;
        rigController.rightHandWeight = 0f;
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
        if (weaponSystem.IsWeaponEquipped)
        {
            rigController.rightHandWeight = 1;
            rigController.leftHandWeight = 0;
        }
        else
        {
            rigController.rightHandWeight = 0;
            rigController.leftHandWeight = 0;
        }
    }

    public void OnExit()
    {
        anim.SetTrigger("land");
        if (weaponSystem.IsWeaponEquipped)
        {
            rigController.rightHandWeight = 1;
            rigController.leftHandWeight = 1;
        }
        else
        {
            rigController.rightHandWeight = 0;
            rigController.leftHandWeight = 0;
        }
    }
}
