using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    [Header("Lock on Target")]
    [SerializeField] CameraController cameraController;
    public Transform currentEnemyTarget;

    public ThirdPersonController thirdPersonController;
    [SerializeField] private Transform attackPositioner;
    [SerializeField] private float dashToAttackTargetDuration = 0.5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController defaultAnimController;

    [Header("Attack Combos")]
    public bool isAttacking = false;
    public List<AttackSO> currentCombo;
    public List<AttackSO> combo_1;
    float lastAttackInputTime;
    float lastComboEnd;
    int comboCount;
    [SerializeField] float timeBetweenAttacks = 0.2f;
    [SerializeField] float timeBetweenCombos = 0.5f;

    private void Start()
    {
        currentCombo = combo_1;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentEnemyTarget != null)
        {
            SetAttackingPositioner_PositionAndRotation();
        }
        ExitAttack();
    }
    //attack logic
    void Attack(List<AttackSO> combo)
    {
        if (Time.time - lastComboEnd > timeBetweenCombos && comboCount <= combo.Count)
        {
            CancelInvoke("EndCombo");
            thirdPersonController.EnableMovement();

            if (Time.time - lastAttackInputTime >= timeBetweenAttacks)
            {
                isAttacking = true;
                animator.runtimeAnimatorController = combo[comboCount].animatorOverride;
                animator.Play("AttackState", 1, 0);

                thirdPersonController.DisableMovement();

                comboCount++;
                lastAttackInputTime = Time.time;
                if (comboCount + 1 > combo.Count)
                {
                    comboCount = 0;
                }
            }
        }
    }
    public void ExitAttack()
    {
        if (isAttacking)
        {
            if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9 && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                thirdPersonController.EnableMovement();
                ForceStopAttack();
            }
            if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5 && thirdPersonController.move.ReadValue<Vector2>().sqrMagnitude > 0.3f)
            {
                thirdPersonController.EnableMovement();
                ForceStopAttack();
            }
        }
    }
    void EndCombo()
    {
        isAttacking = false;
        animator.runtimeAnimatorController = defaultAnimController;
        thirdPersonController.EnableMovement();
        comboCount = 0;
        lastComboEnd = Time.time;
    }
    public void ForceStopAttack()
    {
        isAttacking = false;
        thirdPersonController.EnableMovement();
        animator.runtimeAnimatorController = defaultAnimController;
        Invoke("EndCombo", 1);
    }
    //attack dash
    public void SetPlayerDashPositionForAttack(InputAction.CallbackContext context)
    {
        if (context.started && currentEnemyTarget != null && cameraController.currentCam == cameraController.cinemachine_LockOn)
        {
             //do attack
            Attack(currentCombo);
            //set attack position
            //attackPositioner.GetChild(0).position = new Vector3(0, 0, -currentCombo[comboCount].attackDistanceToEnemy);
           
            //set position
            float distance = Vector3.Distance(transform.position, currentEnemyTarget.transform.position);
            if (distance > 0.2f)
            {
                StartCoroutine(LerpToTargetPosition());
                //rotate player
                Vector3 directionToTarget = transform.position - attackPositioner.position;
                directionToTarget.y = 0f;

                thirdPersonController.forceDirection = -directionToTarget;
                thirdPersonController.rigidbody.velocity = -directionToTarget;
                transform.rotation = Quaternion.LookRotation(-directionToTarget);
            }
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

        if (currentEnemyTarget != null)
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
