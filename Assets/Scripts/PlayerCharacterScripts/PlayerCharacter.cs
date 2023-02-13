using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    Animator anim;
    CharacterController characterController;
    StateMachine state;
    Idle idle;
    Walk walk;
    public Run run;
    public Jump jump;

[SerializeField] InputData inputData;

    [System.Serializable]
    public class CharacterData
    {
        public float speed = 10f;
        public float gravity = -9.81f;
        public float jumpHight = 1f;
        public bool isGrounded = false;
        public float GroundDistance;
    }
    public CharacterData characterData;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        state = GetComponent<StateMachine>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //creating instances of all the states
        idle = new Idle(anim,characterController,characterData);
        walk = new Walk(anim, characterController, inputData, characterData);
        run = new Run(anim,characterController,inputData,characterData);
        jump = new Jump(anim, characterController, inputData,state,this);

        TransitionSetup();

        state.SetState(idle);

    }

    // Update is called once per frame
    void Update()
    {
        characterData.isGrounded = GroundCheck(out characterData.GroundDistance);
    }

    void TransitionSetup()
    {
        //Transitions from any state
        state.AddAnyTransition(jump,JumpCondition);

        //Transitions from walk stats
        At(run, walk, RunCondition);
        At(idle, walk, IdleCondition);

        //transitions from run state
        At(walk, run,WalkCondition);
        At(idle, run,IdleCondition);

        //transitions from idle state
        At(walk, idle, WalkCondition);
        At(run, idle,RunCondition);

        //transitions from jumpstate
        At(run, jump, RunCondition);
        At(walk, jump,WalkCondition);
        At(idle, jump, IdleCondition);

    }
    bool WalkCondition()
    {
        return !inputData.run && (inputData.dpadInput.magnitude > 0f) && characterData.isGrounded;
    }
    bool RunCondition()
    {
        return inputData.run && (inputData.dpadInput.magnitude > 0f) && characterData.isGrounded;
    }

    bool JumpCondition()
    {
        return inputData.jump&&characterData.isGrounded;
    }

    bool IdleCondition()
    {
        return ((inputData.dpadInput.magnitude == 0f) && characterData.isGrounded);
    }

    void At(IState to, IState from, Func<bool> condition) => state.AddTransition(to, from, condition);

    public bool GroundCheck(out float groundDistance)
    {
        float minimalDist = .1f;
        if(Physics.Raycast(transform.position-transform.up*minimalDist*.5f,-transform.up, out RaycastHit hitInfo))
        {
            float distance = hitInfo.distance;
            if(distance<minimalDist)
            {
                groundDistance = distance;
                return true;
            }
            else
            {
                groundDistance = distance;
                return false;
            }
        }
        groundDistance = 1000f;
        return false;
    }
}
