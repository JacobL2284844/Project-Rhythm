using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    [Header("Lock on Target")]
    [SerializeField] CameraController cameraController;
    public Transform currentEnemyTarget;

    [SerializeField] private Transform attackPositioner;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(currentEnemyTarget != null)
        {
            SetAttackingPositionAndRotation();
        }
    }

    public void SetLockOnTarget(Transform targetEnemy)
    {
        currentEnemyTarget = targetEnemy;

        if(currentEnemyTarget != null)
        {

        }
        else
        {

        }
    }

    private void SetAttackingPositionAndRotation()
    {
        //set position
        attackPositioner.position = currentEnemyTarget.position;

        //point towards player
        Vector3 directionToTarget = attackPositioner.position - transform.position;
        directionToTarget.y = 0f; // Ignore changes in height

        if (directionToTarget != Vector3.zero)
        {
            // Rotate towards the target
            attackPositioner.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }
}
