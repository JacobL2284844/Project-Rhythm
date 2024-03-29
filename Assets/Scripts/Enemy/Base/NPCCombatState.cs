using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCCombatState : NPCBaseState
{
    // Start is called before the first frame update
    public NPCStateManager stateManager;

    public NavMeshAgent navMeshAgent;
    public Transform thisNpc;
    public EnemyAnimContext animContext;
    public override void EnterState(NPCStateManager npcContext)
    {
        stateManager.currantStateStr = "Combat";

        stateManager.currantTargetDestination = null;//set destination to null in state manager

        stateManager.navMeshAgent.SetDestination(stateManager.transform.position);

        //anims
        stateManager.currant_animator.SetBool("IsMoving", false);

        //set variables
        thisNpc = stateManager.transform;
        navMeshAgent = stateManager.navMeshAgent;

        RotateFacePlayer();
    }
    public void RotateFacePlayer()
    {
        //rotate face player
        Vector3 direction = new Vector3(stateManager.player.position.x - thisNpc.position.x, 0f, stateManager.player.position.z - thisNpc.position.z);

        // Rotate this object to face towards the target, only on the Y-axis
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        thisNpc.rotation = rotation;
    }
    public void HitReact(AnimatorOverrideController hitOverrideController)
    {
        stateManager.currant_animator.runtimeAnimatorController = hitOverrideController;
        stateManager.currant_animator.Play("CombatEmpty", 1, 0);
    }
    public override void UpdateState(NPCStateManager npcContext)
    {
        //need checks
        //beat checks
    }
}
