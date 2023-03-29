using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    //Inspector Properties
    [SerializeField] Transform[] wayPoints; int wayPointIndex = 0;

    [SerializeField] Transform feedPoint;
    [SerializeField] float patrolDelay = 5f;
    [SerializeField] float agroRange = 10f;
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform eye;
    public enum State { patrol, idle, chase,feed,attack }
    public State state = State.idle;
    [SerializeField]bool usePatrol;


    //private properties
    Animator anim;
    NavMeshAgent agent;
    float idleTime = 0f;

    public bool playerInSight = false;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerInSight = IsPlayerInSight();
        anim.SetFloat("Forward", agent.velocity.sqrMagnitude);
        switch(state)
        {
            case State.feed:

                break;
////**************************************************************////////
            case State.idle:
                if(usePatrol)
                    idleTime += Time.deltaTime;
                ToPatrol();
                ToChase();
                ToAttack();
                break;


////**************************************************************////////
            case State.patrol:
                ToIdle();
                ToChase();
                ToAttack();
                break;


////**************************************************************////////
            case State.chase:
                ToIdle();
                ToAttack();
                ToPatrol();
                agent.SetDestination(playerData.playerPosition);
                break;


 ////**************************************************************////////
            case State.attack:
                ToPatrol();
                ToChase();
                break;
        }
    }


    //Transition Methods
    void ToPatrol()
    {
        if(IsPlayerInSight())
            return;

        if (usePatrol)
        {
            if (idleTime > patrolDelay)
            {
                agent.SetDestination(wayPoints[wayPointIndex].position);
                wayPointIndex++;
                if (wayPointIndex == wayPoints.Length)
                    wayPointIndex = 0;
                anim.SetBool("Run", false);
                state = State.patrol;
                idleTime = 0f;
                
            }
        }
    }
    void ToIdle()
    {
        if(!IsPlayerInSight()&&ReachedDistination())
        {
            state = State.idle;
            anim.SetBool("Run", false);
        }
    }
    void ToChase()
    {
        if(IsPlayerInSight()&&!ReachedDistination())
        {
            state = State.chase;
        }
    }
    void ToAttack()
    {
        if(IsPlayerInSight()&&ReachedDistination())
        {
            state = State.attack;
        }
    }

    bool IsPlayerInSight()
    {
        if (Vector3.Distance(playerData.playerPosition, transform.position) < agroRange)
        {
            Ray ray = new Ray(eye.position, playerData.playerPosition+Vector3.up*1.5f - eye.position);
            Physics.Raycast(ray, out var hit);
            if (hit.transform.CompareTag("Player"))
            {
                agent.SetDestination(playerData.playerPosition);
                return true;
            }
        }
        return false;
    }

    bool ReachedDistination()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
}
