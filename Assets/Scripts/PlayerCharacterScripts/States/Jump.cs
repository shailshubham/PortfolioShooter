using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : IState
{
    Animator anim;
    CharacterController controller;
    InputData inputData;
    StateMachine state;
    PlayerCharacter character;

    Vector3 jumpVelocity= Vector3.zero;
    public Jump
        (
        Animator animator,
        CharacterController characterController,
        InputData inputData,
        StateMachine state,
        PlayerCharacter character
        
        )
    {
        anim = animator;
        controller = characterController;
        this.inputData = inputData;
        this.state= state;
        this.character = character;
    }
    void OnEnter()
    {
        jumpVelocity.y += Mathf.Sqrt(character.characterData.jumpHight * character.characterData.gravity);
        anim.SetTrigger("jump");
    }

    void Update()
    {
        jumpVelocity.y -= character.characterData.gravity * Time.deltaTime;
        controller.Move(jumpVelocity * Time.deltaTime);
        //updating characterMovement
        Vector3 dir = controller.transform.forward * inputData.dpadInput.y + controller.transform.right * inputData.dpadInput.x;
        if(state.previousState == character.run)
             controller.Move(dir * character.characterData.speed * Time.deltaTime);
        else
            controller.Move(dir * character.characterData.speed * Time.deltaTime*.5f);
    }

    void OnExit()
    {

    }
}
