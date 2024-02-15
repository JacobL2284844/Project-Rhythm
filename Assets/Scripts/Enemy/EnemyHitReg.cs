using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitReg : MonoBehaviour
{
    public NPCStateManager stateManager;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (stateManager.currantStateStr == "Chase")
            {
                stateManager.canChangeState = true;
                stateManager.SetState(stateManager.attackState);
            }

            stateManager.canHitPlayer = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (stateManager.currantStateStr == "Attack")
            {
                stateManager.canChangeState = true;
                stateManager.SetState(stateManager.chaseState);
            }

            stateManager.canHitPlayer = false;
        }
    }
}
