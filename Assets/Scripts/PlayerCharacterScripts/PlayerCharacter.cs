using System;
using System.Collections;
using System.Collections.Generic;
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
        public float gravity = 9.81f;
        public float jumpHight = 1f;
    }
    public CharacterData characterData;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        SetupAnimator();
        characterController = GetComponent<CharacterController>();
        state = GetComponent<StateMachine>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //creating instances of all the states
        idle = new Idle(anim);
        walk = new Walk(anim, characterController, inputData, characterData);
        run = new Run(anim,characterController,inputData,characterData);
        jump = new Jump(anim, characterController, inputData,state,this);

        TransitionSetup();

        state.SetState(idle);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void TransitionSetup()
    {
        //Transitions from any state
        state.AddAnyTransition(jump, () => { return inputData.jump; });

        //Transitions from walk stats
        At(run, walk, () => { return inputData.run && (inputData.dpadInput.magnitude > 0f); });
        At(idle, walk, () => { return (inputData.dpadInput.magnitude == 0f); });

        //transitions from run state
        At(walk, run, () => { return !inputData.run && (inputData.dpadInput.magnitude > 0f); });
        At(idle, run, () => { return (inputData.dpadInput.magnitude == 0f); });

        //transitions from idle state
        state.AddTransition(walk, idle,Walk());
        state.AddTransition(run, idle,Run());

        //transitions from jumpstate
        At(run, jump, () => { return inputData.run && (inputData.dpadInput.magnitude > 0f)&&characterController.isGrounded; });
        At(walk, jump, () => { return !inputData.run && (inputData.dpadInput.magnitude > 0f) && characterController.isGrounded; });
        At(idle, jump, () => { return (inputData.dpadInput.magnitude == 0f) && characterController.isGrounded; });

    }

    Func<bool> Walk()=>()=> (!inputData.run && (inputData.dpadInput.magnitude > 0f));
    Func<bool> Run() => () => (inputData.run && (inputData.dpadInput.magnitude > 0f));

    void At(IState to, IState from, Func<bool> condition) => state.AddTransition(to, from, condition);
    void SetupAnimator()
    {
        Animator childAnim = transform.GetChild(0).GetComponent<Animator>();
        Avatar childAvtar = childAnim.avatar;
        anim.avatar = childAvtar;
        Destroy(childAnim);
    }
}
