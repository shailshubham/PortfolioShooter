using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : IState
{
    Animator anim;
    InputData inputData;
    CharacterMover mover;
    AnimationRiggingController rigController;
    WeaponSystem weaponSystem;
    CameraController camController;
    public Aim(PlayerCharacter character)
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
        mover.MoveCharacter(.15f);
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

        weaponSystem.CurrentWeapon.Shoot();
        weaponSystem.CurrentWeapon.Reload();
    }
    public void OnEnter()
    {
        rigController.weaponAimWeight = 1f;
        rigController.weaponDefaultWeight = 0f;
        rigController.AimRigWeight = 1f;

        anim.SetBool("aim", true);
        weaponSystem.CurrentWeaponAnim.SetBool("Aim", true);

        camController.WeaponAim();

        if (weaponSystem.IsWeaponEquipped)
        {
            weaponSystem.CurrentWeaponAnim.SetBool("Run", false);
        }
    }

    public void OnExit()
    {
        rigController.weaponAimWeight = 0f;
        rigController.weaponDefaultWeight = 1f;
        rigController.AimRigWeight = 0f;
        rigController.rightHandWeight = 0;
        rigController.leftHandWeight = 0;


        anim.SetBool("aim", false);
        weaponSystem.CurrentWeaponAnim.SetBool("Aim", false);
        anim.SetFloat("forward", 0f);
        anim.SetFloat("strafe", 0f);
    }
}
