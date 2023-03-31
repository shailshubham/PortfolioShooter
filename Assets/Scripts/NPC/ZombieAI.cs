using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour,IHealth
{
    //Inspector Properties
    [SerializeField] Transform[] wayPoints; int wayPointIndex = 0;
    [SerializeField] int health = 100;
    [SerializeField] Transform feedPoint;
    [SerializeField] float patrolDelay = 5f;
    [SerializeField] float patrolSpeed = .5f;
    [SerializeField] float chaseSpeed = 2f;
    [SerializeField] float agroRange = 10f;
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform eye;
    public enum State { patrol, idle, chase,slowChase,feed,attack,gettingHit,death }
    public State state = State.idle;
    [SerializeField] bool usePatrol;
    [SerializeField] bool feedAtStart;
    [SerializeField] bool screamAtFirstChase; bool screaming = false;


    //private properties
    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent agent;
    ZombieAudio zAudio;
    float timeElapsed = 0f;
    bool isAttacking = false;
    bool isGettingHit = false;
    float animForward = 0f;
    float currentAnimForward = 0f;
    public bool playerInSight = false;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        zAudio = GetComponent<ZombieAudio>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(feedAtStart)
        {
            transform.position = feedPoint.position;
            transform.rotation = feedPoint.rotation;
            usePatrol = false;
            state = State.feed;
            anim.SetBool("Feed", true);
            zAudio.Feed();
        }
    }
 ////**************************************************************////////
    // Update is called once per frame
    void Update()
    {
        playerInSight = IsPlayerInSight();
        currentAnimForward = Mathf.Lerp(currentAnimForward, animForward, .1f);
        anim.SetFloat("Forward", currentAnimForward);
        switch(state)
        {
            case State.feed:
                if(IsPlayerInSight())
                {
                    ToChase();
                    Stop();
                    anim.SetBool("Feed", false);
                    usePatrol = true;
                }
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
                if(!screamAtFirstChase&&!screaming)
                {
                    if (IsPlayerInSight())
                    {
                        agent.SetDestination(playerData.playerPosition);
                        if (IsPlayerInAttackingRange())
                        {
                            ToAttack();
                        }
                    }
                    else
                    {
                        ToSlowChase();
                    }
                    currentAnimForward = 2f;
                }
                else if(!screaming)
                {
                    Stop();
                    anim.SetTrigger("Scream");
                    zAudio.Scream();
                    screaming = true;
                    screamAtFirstChase = false;
                    Invoke("ResetScreamBool", 2.27f);
                }

                break;
////**************************************************************////////
            case State.slowChase:
                if (IsDistinationReached())
                {
                    Stop();
                    ToIdle();
                }
                if (IsPlayerInSight())
                {
                    ToChase();
                }
                animForward = 1f;
                break;


 ////**************************************************************////////
            case State.attack:
                if(IsPlayerInSight()&&!isAttacking)
                {
                    if (IsPlayerInAttackingRange())
                    {
                        Attack();
                    }
                    else
                    {
                        ToChase();
                    }
                }
                else if(!isAttacking)
                {
                    ToSlowChase();
                }
                animForward = 0f;

                break;
////**************************************************************////////
            case State.gettingHit:
                if(!isGettingHit)
                {
                    ToAttack();
                }

                break;
////**************************************************************////////
            case State.death:


                break;
        }//switch statement for state machine finished
    }//update Finished
////**************************************************************////////
    //function
    void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Attack Triggered");
        isAttacking = true;
        zAudio.Attack();
        Invoke("ResetAttackBool", 2.67f);
    }
    void ResetAttackBool()
    {
        isAttacking = false;
    }
    void ResetScreamBool()
    {
        screaming = false;
        ToChase();
    }
    void Stop()
    {
        agent.SetDestination(transform.position);
    }
    public void TakeDamage(int damage)
    {
        if (!isGettingHit)
        {
            health = health - damage;
            if (health <= 0)
            {
                ToDeath();
            }
            else
            {
                anim.SetTrigger("Hit");
                isGettingHit = true;
                Stop();
                state = State.gettingHit;
                zAudio.Hit();
                screamAtFirstChase = false;
                screaming = false;
                Invoke("ResetHitBool", 2.05f);
            }
        }
        else
        {
            health = health - (int)((float)damage * .5f);
            zAudio.Hit();
            if(health<=0)
            {
                ToDeath();
            }
        }
    }

    void ResetHitBool()
    {
        isGettingHit = false;
    }

////**************************************************************////////
    //Transition function
    void ToPatrol()
    {
        if (wayPoints.Length == 0)
            return;
        agent.SetDestination(wayPoints[wayPointIndex].position);
        wayPointIndex++;
        if (wayPointIndex == wayPoints.Length)
            wayPointIndex = 0;
        anim.SetBool("Run", false);
        agent.speed = patrolSpeed;
        state = State.patrol;
        timeElapsed = 0f;
        zAudio.Breath();
    }
    void ToIdle()
    {
        state = State.idle;
        anim.SetBool("Run", false);
        zAudio.Breath();
    }
    void ToChase()
    {
        agent.speed = chaseSpeed;
        state = State.chase;
        zAudio.Breath();
    }
    void ToSlowChase()
    {
        agent.speed = chaseSpeed*.5f;
        agent.SetDestination(playerData.playerPosition);
        state = State.slowChase;
        zAudio.Breath();
    }
    void ToAttack()
    {
        Stop();
        state = State.attack;
    }

    void ToDeath()
    {
        anim.SetBool("Dead", true);
        zAudio.Death();
        state = State.death;
    }
////**************************************************************////////

    //Bool function
    bool IsPlayerInSight()
    {
        if (state == State.death)
            return false;
        if (Vector3.Distance(playerData.playerPosition, transform.position) < agroRange)
        {
            Ray ray = new Ray(eye.position, playerData.playerPosition+Vector3.up*1.5f - eye.position);
            Physics.Raycast(ray, out var hit);
            if (hit.transform.CompareTag("Player"))
            {
                Vector3 dir = playerData.playerPosition - transform.position;
                Quaternion rotation = Quaternion.Euler(0f, Quaternion.LookRotation(dir).eulerAngles.y, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, .1f);
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
