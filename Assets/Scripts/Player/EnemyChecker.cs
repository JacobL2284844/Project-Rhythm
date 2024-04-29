using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    public List<Transform> enemiesInRange = new List<Transform>();
    [SerializeField] private CameraController cameraController;
    private void OnTriggerEnter(Collider other)
    {//add enemy to in range
        if (!enemiesInRange.Contains(other.transform) && other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
            other.GetComponent<NPCStateManager>().animContext.canHearFootstep = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {//remove enemy from in range
        if (enemiesInRange.Contains(other.transform) && other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
            other.GetComponent<NPCStateManager>().animContext.canHearFootstep = false;
        }

        //if out of range of any enemys, remove lock on
        if (enemiesInRange.Count == 0 && cameraController.lockedOn)
        {
            cameraController.SwitchCamera(cameraController.cinemachineFL);
        }

        if(cameraController.lockedOn && cameraController.targetToLock == other)
        {
            cameraController.SwitchCamera(cameraController.cinemachineFL);
        }
    }
}
