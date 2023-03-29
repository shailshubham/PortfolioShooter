using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : IState
{
    Animator anim;
    InputData inputData;
    CharacterMover mover;
    AnimationRiggingController rigController;
    WeaponSystem weaponSystem;
    CameraController camController;
    PlayerData playerData;
    public Run(PlayerCharacter character)
    {
        anim = character.Anim;
        mover = character.CharacterMover;
        inputData = character.inputData;
        rigController = character.RigController;
        weaponSystem = character.weaponSystem;
        camController = character.camController;
        playerData = character.playerData;
    }

    public void Update()
    {
        playerData.isStealthy = false;
        float x = 1f;
        if(inputData.dpadInput.y<0)
        {
            x = .33f;
        }
        mover.MoveCharacter(x);
        anim.SetFloat("forward", inputData.dpadInput.y);
        anim.SetFloat("strafe", inputData.dpadInput.x);
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
        weaponSystem.ChangeWeaponSystem();
        if (weaponSystem.CurrentWeapon != null)
        {
            weaponSystem.CurrentWeapon.Reload();
        }
    }
    public void OnEnter()
    {
        playerData.isStealthy = false;
        camController.RunAim();
        anim.SetBool("run", true);
        rigController.leftHandWeight = 0f;
        if(weaponSystem.IsWeaponEquipped)
        {
            weaponSystem.CurrentWeaponAnim.SetBool("Run", true);
        }
    }

    public void OnExit()
    {
        playerData.isStealthy = true;
        anim.SetBool("run", false);
        anim.SetFloat("forward",0f);
        anim.SetFloat("strafe", 0f);
    }
}
