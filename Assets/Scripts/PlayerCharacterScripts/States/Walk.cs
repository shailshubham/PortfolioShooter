using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : IState
{
    Animator anim;
    CharacterController controller;
    PlayerCharacter.CharacterData characterData;
    InputData inputData;
    public Walk(Animator animator, CharacterController characterController, InputData inputData, PlayerCharacter.CharacterData characterData)
    {
        anim = animator;
        controller= characterController;
        this.characterData = characterData;
        this.inputData = inputData;
    }

        public void Update()
    {
        movement();
    }
    public void OnEnter()
    {

    }

    public void OnExit()
    {
        anim.SetFloat("forward", 0f);
        anim.SetFloat("strafe", 0f);
    }
    void movement()
    {
        Vector3 dir = controller.transform.forward * inputData.dpadInput.y + controller.transform.right * inputData.dpadInput.x;
        controller.Move(dir * characterData.speed * Time.deltaTime*.33f);
        anim.SetFloat("forward", inputData.dpadInput.y);
        anim.SetFloat("strafe", inputData.dpadInput.x);
    }
}
