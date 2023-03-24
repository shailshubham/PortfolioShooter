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
    WeaponSystem weaponSystem;
    CameraController camController;
    public Idle(PlayerCharacter character)
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

        weaponSystem.CurrentWeapon.Reload();
    }
    public void OnEnter() 
    {
        camController.IdleAim();
        anim.SetFloat("forward", 0f);
        anim.SetFloat("strafe", 0f);
        rigController.leftHandWeight = 1f;
        if (weaponSystem.IsWeaponEquipped)
        {
            weaponSystem.CurrentWeaponAnim.SetBool("Run", false);
        }
    }

    public void OnExit()
    { 

    }
}
