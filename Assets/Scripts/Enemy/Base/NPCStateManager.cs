//jacob lueke, state manager script for npc system, last eddited: 08/04/2024
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

[System.Serializable]
public class NPCStateManager : MonoBehaviour
{
    public string currantStateStr = "tbd";//var for keeping track of currant state in editor
    public EnemyMaster enemyMaster;

    [Header("State Machine")]
    public bool isStandardEnemy;
    public bool canChangeState = true;//sometimes set in animation event
    public bool idleOnEntry = false;//sometimes set in animation event
    private NPCBaseState currantState;
    public NPCWanderState wanderState = new NPCWanderState();
    public NPCIdleState idleState = new NPCIdleState();
    public NPCChaseState chaseState = new NPCChaseState();
    public NPCCombatState combatState = new NPCCombatState();

    [Header("NPC")]
    public float npcMood = 1f;// set in spawner
    public Health health;
    public Transform npcMesh;
    public EnemySpawner mySpawner;

    [Header("Animation")]
    public Animator currant_animator;
    public AnimatorOverrideController resetCombatAnimOverrideController;

    [Header("Hit Effect")]
    public ParticleSystem hitEffect;
    public ParticleSystem hitEffect_perfect;

    public Transform hitHeadPosition;
    public Transform hitChestPosition;
    public Transform hitStomachPosition;

    [Header("Navigation")]
    public NavMeshAgent navMeshAgent;
    public float navAgentSpeed = 0.8f;

    public float speed;

    public float idleDuration = 3.5f;
    public float currentIdleDuration = 3.5f;

    public Transform currantTargetDestination;
    public NavigationWanderPoints navigationWanderPoints;

    public float minDestinationDistance = 0.5f;

    [Header("Chase")]
    public float chaseNavAgentSpeed;//used for enemies with legs only
    public Transform player;
    public float turnStrength;
    private Quaternion lookRotation;
    private Vector3 direction;

    public float runNavAgentSpeed = 1.2f;
    public float nav_chaseRunSpeed = 1.2f;


    [Header("Attack")]
    public bool canAttack = false;
    public bool isAttacking = false;
    public bool waitOneBeat = true;//when ready to attack wait one beat
    public bool canHitPlayer = false;

    public float maxAttackDIstance = 3;
    public float attackDamage;

    public List<EnemyAttackSO> myAttacks;

    public EnemyAnimContext animContext;
    public GameObject attackingHitBox;
    public Health playerHealth;

    private void Awake()
    {
        if (isStandardEnemy)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = navAgentSpeed;

            wanderState.navMeshAgent = navMeshAgent;//set variables in states
            wanderState.stateManager = this;
            idleState.stateManager = this;
            chaseState.stateManager = this;
            chaseState.navMeshAgent = navMeshAgent;
            combatState.stateManager = this;
            combatState.navMeshAgent = navMeshAgent;
            combatState.animContext = animContext;
        }
    }
    void Start()
    {
        if (isStandardEnemy)
        {
            if (idleOnEntry)
            {
                SetState(idleState);
            }
            else
            {
                StartCoroutine(SpawnInDelayState());
            }
        }
    }
    IEnumerator SpawnInDelayState()
    {
        SetState(idleState);
        yield return new WaitForSeconds(Random.RandomRange(1, 3));
        currentIdleDuration = 0f;
        SetState(RandomState());
    }
    private void Update()
    {
        if (isStandardEnemy)
        {
            currantState.UpdateState(this); //updates currant state

            if (/*currantState == chaseState || */currantState == wanderState)
            {
                direction = transform.GetComponent<Rigidbody>().velocity.normalized; /*(player.position - transform.position).normalized;*/

                if (direction != Vector3.zero)
                {
                    //rotate in movement direction
                    lookRotation = Quaternion.LookRotation(direction);

                    npcMesh.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
                }
                speed = navMeshAgent.velocity.magnitude;// sets animator walk speed

                if (speed > 0.1)
                {
                    currant_animator.SetBool("IsMoving", true);
                }
                else
                {
                    currant_animator.SetBool("IsMoving", false);
                }
            }
        }
    }
    public NPCBaseState RandomState()//returns a random state
    {
        int index = Random.Range(0, 1);

        if (index == 0)
        {
            currantState = wanderState;
        }
        else
        {
            currantState = idleState;
        }

        return currantState;
    }
    public void SetState(NPCBaseState state)//takes in a provided state (script of type "NPCBaseState")
    {
        //set state logic
        if (canChangeState)
        {
            currantState = state;
            currantState.EnterState(this);

            npcMesh.rotation = Quaternion.identity;//reset rotation
        }
        else
        {
            if (currantState == chaseState)
            {
                return;
            }
            else if (currantState == combatState)
            {
                return;
            }

            currantState = idleState;
            currantState.EnterState(this);

            npcMesh.rotation = Quaternion.identity;//reset rotation
        }

        //set anim combat layer weight
        if (currantState == combatState)
        {
            currant_animator.SetLayerWeight(1, 1);
        }
        else
        {
            currant_animator.SetLayerWeight(1, 0);
        }
    }
    public void StartIdleDuration()
    {
        StartCoroutine(IdleDuration());
    }
    private IEnumerator IdleDuration()
    {
        yield return new WaitForSeconds(currentIdleDuration);
        SetState(RandomState());
    }

    public void LocalBeatCheck()//called in enemy manager/hanadller// called each beat
    {
        if (combatState == currantState && canAttack)//if in combat and ready to attack
        {
            if (waitOneBeat)
            {
                waitOneBeat = false;
                canHitPlayer = true;
                //check if player blocks
            }
            else
            {
                DoAttack();
            }
        }//if player does no input for too long
        //else if(combatState == currantState && enemyMaster.beatClicker.beatsSinceLastInputCheck >= emptyBeatsBeforeAttack && ! canAttack)
        //{
        //    canAttack = true;
        //    waitOneBeat = true;
        //}
    }

    private void DoAttack()//enemy attack start
    {//random attack
        currant_animator.runtimeAnimatorController = myAttacks[Random.Range(0, myAttacks.Count)].animatorOverride;
        currant_animator.Play("CombatEmpty", 1, 0);

        isAttacking = true;
        waitOneBeat = true;
        canAttack = false;


        StartCoroutine(ExitAttack());

        if (canHitPlayer)//set in hit reg
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
    IEnumerator ExitAttack()
    {
        bool attackagain = canHitPlayer; //if last attack succesful attack again
        yield return new WaitForSeconds(0.5f);

        isAttacking = false;

        if (attackagain)
        {
            canAttack = true;
        }
    }
    public void RegisterHit(float damage, AttackManager playerAttackManager, string hitBodyPoint)
    {
        ParticleSystem currentHitEffect;

        //hit effect to use
        if (damage == playerAttackManager.damageForPerfectHit)
        {
            currentHitEffect = hitEffect_perfect;
        }
        else
        {
            currentHitEffect = hitEffect;
        }

        //position hit effect to attack
        switch (hitBodyPoint)
        {
            case "Head":
                currentHitEffect.transform.position = hitHeadPosition.position;
                break;
            case "Chest":
                currentHitEffect.transform.position = hitChestPosition.position;
                break;
            case "Stomach":
                currentHitEffect.transform.position = hitStomachPosition.position;
                break;
            default:
                currentHitEffect.transform.position = hitChestPosition.position;
                break;
        }
        currentHitEffect.Play();
        health.TakeDamage(damage);
    }
}