using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IState CurrentState { get; set; }
    [HideInInspector] public IState previousState;
    public Dictionary<Type,List<Transition>> transitions = new Dictionary<Type,List<Transition>>();
    List<Transition> currentTransitions= new List<Transition>();
    List<Transition> anyTransitions= new List<Transition>();
    List<Transition> emptyTransition = new List<Transition>(0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Tick();
        Debug.Log(CurrentState);
    }

    private void FixedUpdate()
    {
        IStateFixedUpdate state = CurrentState as IStateFixedUpdate;
        if (state != null)
        {
            state.FixedUpdate();
        }
    }

    void Tick()
    {
        Transition t = GetTransitions();
        if (t != null)
        {
            SetState(t.To);
        }
        CurrentState.Update();
    }

    public void SetState(IState state)
    {
        if(state == CurrentState) return;
        if (CurrentState != null)
            CurrentState.OnExit();
        previousState= state;
        CurrentState = state;

        transitions.TryGetValue(CurrentState.GetType(), out currentTransitions);
        if (currentTransitions == null)
            currentTransitions = emptyTransition;
        CurrentState.OnEnter();      
    }

    [System.Serializable]
    public class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }
        public Transition(IState to, Func<bool> condition)
        {
            Condition = condition;
            To = to;
        }
    }

    public void AddTransition(IState to,IState from,Func<bool> condition)
    {
        if(transitions.TryGetValue(from.GetType(),out var trans)==false)
        {
            trans = new List<Transition>();
            transitions[from.GetType()] = trans;
        }
        trans.Add(new Transition(to, condition));
    }

    public void AddAnyTransition(IState to,Func<bool> condition)
    {
        anyTransitions.Add(new Transition(to, condition));
    }

    Transition GetTransitions()
    {
        foreach(var t in anyTransitions) 
        {
            if (t.Condition())
                return t;
        }
        foreach (var t in currentTransitions)
        {
            if (t.Condition())
                return t;
        }
        return null;
    }
}