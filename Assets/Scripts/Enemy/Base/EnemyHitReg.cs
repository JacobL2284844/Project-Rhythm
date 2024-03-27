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
            stateManager.canHitPlayer = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            stateManager.canHitPlayer = false;
        }
    }
}
