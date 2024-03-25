using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAttackState : NPCBaseState
{
    public NPCStateManager stateManager;
    public NavMeshAgent navMeshAgent;
    public EnemyAnimContext animContext;

    public Transform playerPos;
    public Transform thisNpc;
    public Transform npcMesh;

    public float turnStrength;
    private Quaternion lookRotation;
    private Vector3 direction;

    public override void EnterState(NPCStateManager npcContext)//works as start
    {
        stateManager.currantStateStr = "Attack";
        stateManager.canChangeState = false;

        npcMesh = stateManager.npcMesh;

        Debug.Log("enter attack");
        //set variables
        thisNpc = stateManager.transform;

        playerPos = stateManager.player;// set player as destination
        stateManager.currantTargetDestination = playerPos;//set destination on manager

        //set var agent speeda
        navMeshAgent.speed = 0f;

        //set anim move
        stateManager.currant_animator.SetBool("IsMoving", false);


    }

    public override void UpdateState(NPCStateManager npcContext)//works as start
    {
        direction = npcMesh.position - playerPos.position; /*(player.position - transform.position).normalized;*/
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            //npcMesh.rotation = Quaternion.LookRotation(-direction);
        }


        if (stateManager.canHitPlayer && ! animContext.isAttacking)
        {
            animContext.DoRandomMelleeAttack();
        }
    }

    public float DistancToTargetPos()
    {
        // calculate the distance between the two objects
        return Vector3.Distance(thisNpc.position, playerPos.position);
    }
}
