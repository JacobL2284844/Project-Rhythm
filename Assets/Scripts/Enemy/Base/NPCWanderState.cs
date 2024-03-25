//jacob lueke, wander state for npc's, defines where to walk to. date started- need to check
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWanderState : NPCBaseState// script of type state
{
    public NPCStateManager stateManager;
    public NavMeshAgent navMeshAgent;

    public Transform currantTargetDestination;
    public Transform thisNpc;

    private float minDestinationDistance = 0.5f;

    public override void EnterState(NPCStateManager npcContext)//works as start
    {
        stateManager = npcContext;

        stateManager.currantStateStr = "Wander";

        //set variables
        minDestinationDistance = stateManager.minDestinationDistance;
        thisNpc = stateManager.transform;

        //get destination
        currantTargetDestination = stateManager.navigationWanderPoints.GetRandomPoint();
        stateManager.currantTargetDestination = currantTargetDestination;//set destination on manager

        try// try set destination on navmesh
        {
            navMeshAgent.destination = currantTargetDestination.position;// set new destination
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

    public override void UpdateState(NPCStateManager npcContext)//works as update
    {

        if (!navMeshAgent.pathPending)
        {
            // calculate the distance between the two objects
            float distance = Vector3.Distance(thisNpc.position, currantTargetDestination.position);

            // check if the distance is below the threshold
            if (distance <= minDestinationDistance)// if yes
            {
                stateManager.SetState(stateManager.idleState);// set state to idle
            }

            if (navMeshAgent.speed < 0.5f)// if walking to destination there is nav error
            {
                stateManager.SetState(stateManager.idleState);// set state to idle
            }
        }
    }
}