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

    [Header("Attack Damage")]
    public float damageForHit = 10;
    public float damageForPerfectHit = 20;


    [Header("Attack Combos")]
    public bool isAttacking = false;
    public List<AttackSO> currentCombo;
    public List<ComboSO> combos;
    public AttackSO block;

    public BeatClicker beatClicker;

    float lastAttackInputTime;
    float lastComboEnd;
    [SerializeField] int comboCount;
    [SerializeField] float timeBetweenAttacks = 0.2f;
    [SerializeField] float timeBetweenCombos = 0.5f;
    [SerializeField] float fovChangeOnAttack = 25;

    private void Start()
    {
        SetRandomCombo();
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
    void Attack(List<AttackSO> combo)//will need to take in beat check timing
    {
        CancelInvoke("EndCombo");
        thirdPersonController.EnableMovement();

        if (Time.time - lastAttackInputTime >= timeBetweenAttacks)
        {
            isAttacking = true;
            attackPositioner.GetChild(0).localPosition = new Vector3(0, 0, -combo[comboCount].attackDistanceToEnemy);
            animator.runtimeAnimatorController = combo[comboCount].animatorOverride;
            animator.Play("AttackState", 1, 0);

            thirdPersonController.DisableMovement();

            comboCount++;
            lastAttackInputTime = Time.time;
            if (comboCount + 1 > combo.Count)
            {
                comboCount = 0;
                SetRandomCombo();
            }

            //on enemy hit
            NPCStateManager stateManager = currentEnemyTarget.GetComponent<NPCStateManager>();

            if (stateManager.currantStateStr != "Combat")
            {
                stateManager.SetState(stateManager.combatState);

                HitEnemy(combo);
            }
            else
            {
                HitEnemy(combo);
            }
        }
    }

    void HitEnemy(List<AttackSO> combo)
    {
        NPCStateManager stateManager = currentEnemyTarget.GetComponent<NPCStateManager>();
        //rotate enemy to face player
        stateManager.combatState.RotateFacePlayer();

        //animate hit react
        int hitStrenth = CheckBeatAccuracy();//get strength from input timing

        AnimatorOverrideController enemysHitReactAnim = combo[comboCount].enemyReactions[hitStrenth];
        stateManager.combatState.HitReact(enemysHitReactAnim);

        //get location for hit effect on enemy from attack
        string hitOnBodyLocation = combo[comboCount].hitOnBodyLocation;

        switch (hitStrenth)
        {
            case 0://player misses 
                stateManager.canAttack = true;//if enemy blocks hit enemy can attack
                break;
            case 1://good hit
                stateManager.RegisterHit(damageForHit, this, hitOnBodyLocation);
                break;
            case 2://perfect hit
                stateManager.RegisterHit(damageForPerfectHit, this, hitOnBodyLocation);
                break;
            default:
                break;
        }
    }
    int CheckBeatAccuracy()//0 miss, 1 good, 2 perfect. classes hit strength
    {
        string hitState = beatClicker.recentHitState;

        if (hitState == beatClicker.perfectTag)
        {
            return 2;
        }
        else if (hitState == beatClicker.goodTag || hitState == beatClicker.mehTag)
        {
            return 1;
        }
        else if (hitState == beatClicker.failTag)
        {
            return 0;
        }
        else
        {
            return 0;
        }
    }
    public void Block()
    {
        if (Time.time - lastComboEnd > timeBetweenCombos)
        {
            CancelInvoke("EndCombo");
            thirdPersonController.EnableMovement();

            if (Time.time - lastAttackInputTime >= timeBetweenAttacks)
            {
                if (CheckBeatAccuracy() != 0 && currentEnemyTarget.GetComponent<NPCStateManager>().canAttack)
                {//if enemy attacking and player blocked on beat
                    Debug.Log("BlockOnBeat");

                    if (currentEnemyTarget.GetComponent<NPCStateManager>())
                    {
                        currentEnemyTarget.GetComponent<NPCStateManager>().canHitPlayer = false;
                    }
                }
                else
                {
                    currentEnemyTarget.GetComponent<NPCStateManager>().canHitPlayer = true;
                    Debug.Log("Block Failed");
                }
                isAttacking = true;
                attackPositioner.GetChild(0).localPosition = new Vector3(0, 0, -block.attackDistanceToEnemy);
                animator.runtimeAnimatorController = block.animatorOverride;
                animator.Play("AttackState", 1, 0);

                thirdPersonController.DisableMovement();

                lastAttackInputTime = Time.time;
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
            }//should code timings to bpm ?
            if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5 && thirdPersonController.move.ReadValue<Vector2>().sqrMagnitude > 0.3f)
            {
                thirdPersonController.EnableMovement();
                ForceStopAttack();
            }
        }
    }
    void EndCombo()
    {
        //Debug.Log("End Combo");

        isAttacking = false;
        animator.runtimeAnimatorController = defaultAnimController;
        thirdPersonController.EnableMovement();
        comboCount = 0;
        lastComboEnd = Time.time;
    }
    public void ForceStopAttack()
    {
        isAttacking = false;
        attackPositioner.GetChild(0).localPosition = new Vector3(0, 0, -1);
        thirdPersonController.EnableMovement();
        animator.runtimeAnimatorController = defaultAnimController;
        EndCombo();
    }

    void SetRandomCombo()
    {
        //Debug.Log("Random combo");
        currentCombo.Clear();

        int index = Random.Range(0, combos.Count);

        foreach (var attack in combos[index].combo)//random combo
        {
            currentCombo.Add(attack);
        }
    }
    //attack dash when attack is first called from input
    public void PerformAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {//if not locked
            if (!currentEnemyTarget || cameraController.currentCam != cameraController.cinemachine_LockOn)
            {
                cameraController.TargetClosestEnemy();
                cameraController.SetTargetClosestAndLockOn();
                SetAttackingPositioner_PositionAndRotation();
            }
            //once locked
            if (currentEnemyTarget != null && cameraController.currentCam == cameraController.cinemachine_LockOn)
            {//combo timing
                if (Time.time - lastComboEnd > timeBetweenCombos && comboCount <= currentCombo.Count)
                {
                    NPCStateManager stateManager = currentEnemyTarget.GetComponent<NPCStateManager>();

                    if (stateManager.canAttack)//enemy is attacking
                    {
                        //set attack positioner position to specific attack
                        attackPositioner.GetChild(0).localPosition = new Vector3(0, 0, -currentCombo[comboCount].attackDistanceToEnemy);

                        //do block
                        Block();
                    }
                    else
                    {
                        //set attack positioner position to specific attack
                        attackPositioner.GetChild(0).localPosition = new Vector3(0, 0, -currentCombo[comboCount].attackDistanceToEnemy);
                        //do attack
                        Attack(currentCombo);
                    }



                    //set position
                    float distance = Vector3.Distance(transform.position, currentEnemyTarget.transform.position);
                    if (distance > 0.1f)
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
        }
    }

    IEnumerator LerpToTargetPosition()
    {
        Vector3 initialPosition = transform.position;

        //fov
        cameraController.transitionSpeed = cameraController.transitionSpeed / 10;
        cameraController.maxFOV = cameraController.maxFOV - fovChangeOnAttack;
        cameraController.minFOV = cameraController.minFOV - fovChangeOnAttack;

        float elapsedTime = 0f;
        while (elapsedTime < dashToAttackTargetDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, attackPositioner.GetChild(0).position, elapsedTime / dashToAttackTargetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //fov
        cameraController.transitionSpeed = cameraController.transitionSpeed * 10;
        cameraController.maxFOV = cameraController.maxFOV + fovChangeOnAttack;
        cameraController.minFOV = cameraController.minFOV + fovChangeOnAttack;

        // Ensure reaching exact target position
        transform.position = attackPositioner.GetChild(0).position;

        //rotate player
        Vector3 directionToTarget = transform.position - attackPositioner.position;
        directionToTarget.y = 0f;

        thirdPersonController.forceDirection = -directionToTarget;
        thirdPersonController.rigidbody.velocity = -directionToTarget;
        transform.rotation = Quaternion.LookRotation(-directionToTarget);
    }
    public void SetLockOnTarget(Transform targetEnemy)
    {
        currentEnemyTarget = targetEnemy;

        if (currentEnemyTarget != null)
        {
            //checks ??
        }
        else
        {

        }
    }

    private void SetAttackingPositioner_PositionAndRotation()
    {
        //set position
        if (currentEnemyTarget != null)
        {
            attackPositioner.position = currentEnemyTarget.position;
        }

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
