using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCharacter;

public class Jump : IState
{
    Animator anim;
    CharacterController controller;
    InputData inputData;
    StateMachine state;
    PlayerCharacter character;
    bool land = false;

    Vector3 velocity= Vector3.zero;
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
    public void OnEnter()
    {
        land = false;
        velocity.y = Mathf.Sqrt(character.characterData.jumpHight * -character.characterData.gravity);
        if (state.previousState == character.run)
            anim.SetTrigger("runJump");
        else
            anim.SetTrigger("jump");
    }

    public void Update()
    {
        if(character.characterData.isGrounded&&velocity.y<0)
        {
            velocity.y = -2;
        }
        velocity.y += character.characterData.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (character.characterData.GroundDistance < 1f&&!land)
        {
            anim.SetTrigger("land");
            land = true;
        }
        //updating characterMovement
        Vector3 dir = controller.transform.forward * inputData.dpadInput.y + controller.transform.right * inputData.dpadInput.x;
        if(state.previousState == character.run)
            controller.Move(dir * character.characterData.speed * Time.deltaTime);
        else
            controller.Move(dir * character.characterData.speed * Time.deltaTime*.5f);

    }

    public void OnExit()
    {

    }
}
