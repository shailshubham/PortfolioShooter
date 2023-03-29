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
    [SerializeField] float patrolSpeed = .5f;
    [SerializeField] float chaseSpeed = 2f;
    [SerializeField] float agroRange = 10f;
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform eye;
    public enum State { patrol, idle, chase,feed,attack }
    public State state = State.idle;
    [SerializeField]bool usePatrol;


    //private properties
    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent agent;
    float timeElapsed = 0f;
    bool isAttacking = false;
    float animForward = 0f;
    float currentAnimForward = 0f;
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
        currentAnimForward = Mathf.Lerp(currentAnimForward, animForward, 1f);
        anim.SetFloat("Forward", currentAnimForward);
        switch(state)
        {
            case State.feed:

                break;
////**************************************************************////////
            case State.idle:
                if(usePatrol&&!IsPlayerInSight())
                {
                    timeElapsed += Time.deltaTime;
                    if(timeElapsed>patrolDelay)
                    {
                        timeElapsed = 0;
                        ToPatrol();
                    }
                }
                if(IsPlayerInSight())
                {
                    timeElapsed = 0;
                    ToChase();
                }
                animForward = 0f;
                break;


////**************************************************************////////
            case State.patrol:
                if(IsDistinationReached()&&!IsPlayerInSight())
                {
                    ToIdle();
                }
                if (IsPlayerInSight())
                {
                    ToChase();
                }
                animForward = .5f;
                break;


////**************************************************************////////
            case State.chase:
                if(IsPlayerInSight())
                {
                    agent.SetDestination(playerData.playerPosition);
                    if (IsDistinationReached())
                    {
                        ToAttack();
                    }
                }
                else
                {
                    Stop();
                    ToIdle();
                }
                animForward = 1f;
                break;


 ////**************************************************************////////
            case State.attack:
                if(IsPlayerInSight())
                {
                    if (IsPlayerInAttackingRange())
                    {
                        Attack(true);
                    }
                    else if(!isAttacking)
                    {
                        Attack(false);
                        ToChase();
                    }
                }
                else if(!isAttacking)
                {
                    Attack(false);
                    Stop();
                    ToIdle();
                }
                animForward = 0f;

                break;
        }
    }

    //function
    void Attack(bool attack)
    {
        anim.SetBool("Attack",attack);
        isAttacking = true;
        Invoke("ResetAttackBool", 3f);
    }
    void ResetAttackBool()
    {
        isAttacking = false;
    }
    void Stop()
    {
        agent.SetDestination(transform.position);
    }

    //Transition function
    void ToPatrol()
    {
        agent.SetDestination(wayPoints[wayPointIndex].position);
        wayPointIndex++;
        if (wayPointIndex == wayPoints.Length)
            wayPointIndex = 0;
        anim.SetBool("Run", false);
        agent.speed = patrolSpeed;
        state = State.patrol;
        timeElapsed = 0f;
    }
    void ToIdle()
    {
        state = State.idle;
        anim.SetBool("Run", false);
    }
    void ToChase()
    {
        agent.speed = chaseSpeed;
        state = State.chase;
    }
    void ToAttack()
    {
        Stop();
        state = State.attack;
    }


    //Bool function
    bool IsPlayerInSight()
    {
        if (Vector3.Distance(playerData.playerPosition, transform.position) < agroRange)
        {
            Ray ray = new Ray(eye.position, playerData.playerPosition+Vector3.up*1.5f - eye.position);
            Physics.Raycast(ray, out var hit);
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    bool IsPlayerInAttackingRange()
    {
        return Vector3.Distance(playerData.playerPosition,transform.position)<=agent.stoppingDistance;
    }

    bool IsDistinationReached()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
}
