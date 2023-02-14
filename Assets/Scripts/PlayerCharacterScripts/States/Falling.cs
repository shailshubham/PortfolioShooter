using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : IState
{
    Animator anim;
    InputData inputData;
    CharacterMover mover;
    AnimationRiggingController rigController;
    public Falling(PlayerCharacter character)
    {
        anim = character.Anim;
        mover = character.CharacterMover;
        inputData = character.inputData;
        rigController = character.RigController;
    }
    public void OnEnter()
    {

    }

    public void Update()
    {
        
    }

    public void OnExit()
    {

    }
}
