using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public CharacterMover CharacterMover { get; private set; }
    public AnimationRiggingController RigController { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public WeaponSystem weaponSystem { get; private set; }
    public CameraController camController { get; private set; }
    Idle idle;
    Walk walk;
    Aim aim;
    public Run run;
    public Jump jump;
    [SerializeField] InputData input;
    public InputData inputData
    {
        get { return input; }
    }
    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        //SetupAnimator();
        CharacterMover = GetComponent<CharacterMover>();
        RigController = GetComponent<AnimationRiggingController>();
        StateMachine = GetComponent<StateMachine>();
        weaponSystem = GetComponent<WeaponSystem>();
        camController = GetComponent<CameraController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //creating instances of all the states
        idle = new Idle(this);
        walk = new Walk(this);
        run = new Run(this);
        jump = new Jump(this); ;
        aim = new Aim(this);
        TransitionSetup();

        StateMachine.SetState(idle);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TransitionSetup()
    {
        //Transitions from any state
        StateMachine.AddAnyTransition(jump,JumpCondition);

        //Transitions from walk stats
        At(run, walk, RunCondition);
        At(idle, walk, IdleCondition);
        At(aim, walk, AimCondition);

        //transitions from run state
        At(walk, run,WalkCondition);
        At(idle, run,IdleCondition);
        At(aim, run, AimCondition);

        //transitions from idle state
        At(walk, idle, WalkCondition);
        At(run, idle,RunCondition);
        At(aim, idle, AimCondition);

        //transitions from jumpstate
        At(run, jump, RunCondition);
        At(walk, jump,WalkCondition);
        At(idle, jump, IdleCondition);

        //transitions from aimstate
        At(run, aim, RunCondition);
        At(walk, aim, WalkCondition);
        At(idle, aim, IdleCondition);

    }
    bool WalkCondition()
    {
        return !input.run && (input.dpadInput.magnitude > 0f) && CharacterMover.isGrounded&&!inputData.Aim;
    }
    bool RunCondition()
    {
        return input.run && (input.dpadInput.magnitude > 0f) && CharacterMover.isGrounded && !inputData.Aim;
    }

    bool JumpCondition()
    {
        return input.jump&& CharacterMover.isGrounded;
    }

    bool IdleCondition()
    {
        return ((input.dpadInput.magnitude == 0f) && CharacterMover.isGrounded) && !inputData.Aim;
    }

    bool AimCondition()
    {
        return inputData.Aim;
    }

    void At(IState to, IState from, Func<bool> condition) => StateMachine.AddTransition(to, from, condition);

    void SetupAnimator()
    {
        Animator childAnim = transform.GetChild(0).GetComponent<Animator>();
        Anim.avatar = childAnim.avatar;
        childAnim.avatar = null;
    }
}
