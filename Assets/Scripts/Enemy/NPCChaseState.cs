using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCChaseState : NPCBaseState
{
    public NPCStateManager stateManager;
    public NavMeshAgent navMeshAgent;

    public Transform playerPos;
    public Transform thisNpc;

    public override void EnterState(NPCStateManager npcContext)//works as start
    {
        stateManager.currantStateStr = "Chase";
        stateManager.canChangeState = false;

        //set variables
        thisNpc = stateManager.transform;

        playerPos = stateManager.player;// set player as destination
        stateManager.currantTargetDestination = playerPos;//set destination on manager

        if (stateManager.hasLegs)
        {
            stateManager.currant_animator.SetTrigger("Chase");

            if(stateManager.animContext.isStanding)
            {
                stateManager.navMeshAgent.speed = stateManager.chaseNavAgentSpeed;
            }
        }
        else
        {
            stateManager.navMeshAgent.speed = stateManager.chaseNavAgentSpeed;
        }

        try// try set destination on navmesh
        {
            navMeshAgent.destination = playerPos.position;// set new destination
        }
        catch// if nav agent not on navmesh, an error normally happens
        {
            RaycastHit hit;
            if (Physics.Raycast(thisNpc.position, Vector3.down, out hit, 3/*ray distance*/))
            {
                // set the position of the capsule to be on top of the hit point
                thisNpc.position = hit.point + Vector3.up * thisNpc.localScale.y / 2;
            }
        }
    }

    public override void UpdateState(NPCStateManager npcContext)//works as start
    {
        if (!navMeshAgent.pathPending)
        {
            navMeshAgent.destination = playerPos.position;//set destination to player
        }
    }
}