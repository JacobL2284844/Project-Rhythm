//jacob lueke, idle state for npc's. date started- need to check
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCIdleState : NPCBaseState// script of type state
{
    public NPCStateManager stateManager;

    public NavMeshAgent navMeshAgent;
    public Transform thisNpc;
    public override void EnterState(NPCStateManager npcContext)
    {
        stateManager.currantStateStr = "Idle";

        stateManager.currantTargetDestination = null;//set destination to null in state manager

        stateManager.navMeshAgent.SetDestination(stateManager.transform.position);

        stateManager.currentIdleDuration = stateManager.idleDuration;

        stateManager.StartIdleDuration();

        //anims

        if (stateManager.hasLegs)
        {
            stateManager.currant_animator.SetBool("IsMoving", false);
        }

        //set variables
        thisNpc = stateManager.transform;
        navMeshAgent = stateManager.navMeshAgent;

        //try// try set destination on navmesh
        //{
        //    navMeshAgent.destination = stateManager.transform.position;// set new destination
        //}
        //catch// if nav agent not on navmesh, an error normally happens
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(thisNpc.position, Vector3.down, out hit, 3/*ray distance*/))
        //    {
        //        // set the position of the capsule to be on top of the hit point
        //        thisNpc.position = hit.point + Vector3.up * thisNpc.localScale.y / 2;
        //    }
        //}
    }

    public override void UpdateState(NPCStateManager npcContext)
    {
        //need checks
    }
}
