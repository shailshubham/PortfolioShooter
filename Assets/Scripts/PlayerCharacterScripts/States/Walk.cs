using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : IState
{
    Animator anim;
    InputData inputData;
    CharacterMover mover;
    AnimationRiggingController rigController;
    WeaponSystem weaponSystem;
    CameraController camController;
    public Walk(PlayerCharacter character)
    {
        anim = character.Anim;
        mover = character.CharacterMover;
        inputData = character.inputData;
        rigController = character.RigController;
        weaponSystem = character.weaponSystem;
        camController = character.camController;
    }

        public void Update()
    {
        mover.MoveCharacter(.33f);
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
        anim.SetFloat("forward", inputData.dpadInput.y);
        anim.SetFloat("strafe", inputData.dpadInput.x);
    }
    public void OnEnter()
    {
        camController.IdleAim();
        if (weaponSystem.IsWeaponEquipped)
        {
            weaponSystem.CurrentWeaponAnim.SetBool("Run", false);
        }
    }

    public void OnExit()
    {
        anim.SetFloat("forward", 0f);
        anim.SetFloat("strafe", 0f);
    }
}
