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

    //states
    Idle idle;
    Walk walk;
    Aim aim;
    public Run run;
    public Jump jump;
    Reloading reloading;

    [SerializeField] InputData input;
    public InputData inputData
    {
        get { return input; }
    }
    public PlayerData playerData;
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
        reloading = new Reloading(this);
        
        TransitionSetup();

        StateMachine.SetState(idle);

    }

    // Update is called once per frame
    void Update()
    {
        playerData.playerPosition = transform.position;
    }

    void TransitionSetup()
    {

        //Transitions from walk stats
        At(run, walk, RunCondition);
        At(idle, walk, IdleCondition);
        At(aim, walk, AimCondition);
        At(jump, walk, JumpCondition);
        At(reloading, walk, ReloadingCondition);

        //transitions from run state
        At(walk, run,WalkCondition);
        At(idle, run,IdleCondition);
        At(aim, run, AimCondition);
        At(jump, run, JumpCondition);
        At(reloading, run, ReloadingCondition);


        //transitions from idle state
        At(walk, idle, WalkCondition);
        At(run, idle,RunCondition);
        At(aim, idle, AimCondition);
        At(jump, idle, JumpCondition);
        At(reloading, idle, ReloadingCondition);


        //transitions from jumpstate
        At(run, jump, RunCondition);
        At(walk, jump,WalkCondition);
        At(idle, jump, IdleCondition);
        At(aim, jump, AimCondition);

        //transitions from aimstate
        At(run, aim, RunCondition);
        At(walk, aim, WalkCondition);
        At(idle, aim, IdleCondition);
        At(reloading, aim, ReloadingCondition);

        //transitions from Reloading State
        At(walk, reloading, WalkCondition);
        At(run, reloading, RunCondition);
        At(idle, reloading, IdleCondition);
        At(aim, reloading, AimCondition);
    }
    bool WalkCondition()
    {
        bool isReloading = weaponSystem.CurrentWeapon ? weaponSystem.CurrentWeapon.Reloading : false;
        return 
            !input.run 
            && (input.dpadInput.magnitude > 0f) 
            && CharacterMover.isGrounded
            &&!inputData.Aim
            &&!isReloading;
    }
    bool RunCondition()
    {
        bool isReloading = weaponSystem.CurrentWeapon ? weaponSystem.CurrentWeapon.Reloading : false;

        return
            input.run 
            && (input.dpadInput.magnitude > 0f) 
            && CharacterMover.isGrounded 
            && !inputData.Aim
            && !isReloading;
    }

    bool JumpCondition()
    {
        bool isReloading = weaponSystem.CurrentWeapon ? weaponSystem.CurrentWeapon.Reloading : false;

        return
            input.jump
            && CharacterMover.isGrounded
            &&!isReloading;
    }

    bool IdleCondition()
    {
        bool isReloading = weaponSystem.CurrentWeapon ? weaponSystem.CurrentWeapon.Reloading : false;

        return
            input.dpadInput.magnitude == 0f 
            && CharacterMover.isGrounded 
            && !inputData.Aim 
            && !isReloading;
    }

    bool AimCondition()
    {
        return 
            inputData.Aim&&weaponSystem.IsWeaponEquipped
            &&CharacterMover.isGrounded;
    }
    bool ReloadingCondition()
    {
        bool isReloading = weaponSystem.CurrentWeapon ? weaponSystem.CurrentWeapon.Reloading : false;
        return 
            isReloading;
    }

    void At(IState to, IState from, Func<bool> condition) => StateMachine.AddTransition(to, from, condition);

    void SetupAnimator()
    {
        Animator childAnim = transform.GetChild(0).GetComponent<Animator>();
        Anim.avatar = childAnim.avatar;
        childAnim.avatar = null;
    }
}
