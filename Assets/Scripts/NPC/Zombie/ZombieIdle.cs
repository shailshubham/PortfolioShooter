using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieIdle : IState
{
    Animator anim;
    NavMeshAgent agent;
    ZombieAI ai;
    public ZombieIdle(ZombieAI zombie)
    {
        ai = zombie;
        anim = zombie.anim;
        agent = zombie.agent;
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
