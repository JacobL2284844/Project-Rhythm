using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    [Header("Lock on Target")]
    [SerializeField] CameraController cameraController;
    public Transform currentEnemyTarget;

    [SerializeField] ThirdPersonController thirdPersonController;
    [SerializeField] private Transform attackPositioner;
    [SerializeField] private float dashToAttackTargetDuration = 0.5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(currentEnemyTarget != null)
        {
            SetAttackingPositioner_PositionAndRotation();
        }
    }

    public void SetPlayerDashPositionForAttack(InputAction.CallbackContext context)
    {
        if (context.started && currentEnemyTarget != null)
        {
            //set position
            StartCoroutine(LerpToTargetPosition());
            //rotate player
            Vector3 directionToTarget = transform.position - attackPositioner.position;
            directionToTarget.y = 0f;

            thirdPersonController.forceDirection = -directionToTarget;
            thirdPersonController.rigidbody.velocity = -directionToTarget;
            transform.rotation = Quaternion.LookRotation(-directionToTarget);
        }
    }
    IEnumerator LerpToTargetPosition()
    {
        Vector3 initialPosition = transform.position;

        //fov
        cameraController.transitionSpeed = cameraController.transitionSpeed / 10;
        cameraController.maxFOV = cameraController.maxFOV - 25;//should not be hard coded
        cameraController.minFOV = cameraController.minFOV - 25;

        float elapsedTime = 0f;
        while (elapsedTime < dashToAttackTargetDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, attackPositioner.GetChild(0).position, elapsedTime / dashToAttackTargetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //fov
        cameraController.transitionSpeed = cameraController.transitionSpeed * 10;
        cameraController.maxFOV = cameraController.maxFOV + 25;//should not be hard coded
        cameraController.minFOV = cameraController.minFOV + 25;

        transform.position = attackPositioner.GetChild(0).position; // Ensure reaching exact target position
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

    private void SetAttackingPositioner_PositionAndRotation()
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
