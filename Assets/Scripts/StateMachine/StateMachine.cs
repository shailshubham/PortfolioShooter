using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IState currentState;
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
    }

    private void FixedUpdate()
    {
        IStateFixedUpdate state = currentState as IStateFixedUpdate;
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
        currentState.Update();
    }

    public void SetState(IState state)
    {
        if(state == currentState) return;
        if (currentState != null)
            currentState.OnExit();
        previousState= currentState;
        currentState = state;

        transitions.TryGetValue(currentState.GetType(), out currentTransitions);
        if (currentTransitions == null)
            currentTransitions = emptyTransition;
        currentState.OnEnter();      
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