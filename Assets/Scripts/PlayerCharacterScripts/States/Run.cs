using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : IState
{
    Animator anim;
    CharacterController controller;
    PlayerCharacter.CharacterData characterData;
    InputData inputData;
    public Run(Animator animator,CharacterController characterController, InputData inputData,PlayerCharacter.CharacterData characterData)
    {
        anim = animator;
        controller= characterController;
        this.characterData = characterData;
        this.inputData = inputData;
    }

    public void Update()
    {
        movement();
        Gravity();
    }
    void OnEnter()
    {

    }

    void OnExit()
    {
        anim.SetFloat("forward",0f);
        anim.SetFloat("strafe", 0f);
    }

    void movement()
    {
        Vector3 dir = controller.transform.forward * inputData.dpadInput.y + controller.transform.right * inputData.dpadInput.x;
        controller.Move(dir * characterData.speed * Time.deltaTime);
        anim.SetFloat("forward", inputData.dpadInput.y);
        anim.SetFloat("strafe", inputData.dpadInput.x);
    }

    void Gravity()
    {
        controller.Move(Vector3.down * characterData.gravity * Time.deltaTime);
    }
}
