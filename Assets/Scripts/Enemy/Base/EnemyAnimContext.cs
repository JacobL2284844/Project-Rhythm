using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimContext : MonoBehaviour
{
    public NPCStateManager stateManager;

    public string[] attackTriggers;

    public bool isAttacking;
    public bool isStanding = false;

    public Health playerHealth;

    private void Start()
    {
        playerHealth = stateManager.player.GetComponent<Health>();
    }
    public void AttackStart()
    {
        isAttacking = true;
    }
    public void AttackEnd()
    {
        isAttacking = false;
    }

    //ATTACKS
    public void CheckHitReg()//point in animation when damage is dealt, call hit reg
    {//damage enemys regestered in specific collider
        if (stateManager.canHitPlayer)
        {
            playerHealth.TakeDamage(stateManager.attackDamage);
        }
    }
    public void DoRandomMelleeAttack()
    {
        isAttacking = true;

        int randomIndex = Random.Range(0, attackTriggers.Length);

        stateManager.currant_animator.SetTrigger(attackTriggers[randomIndex]);
    }


    public void StandUpStart()
    {
        stateManager.navMeshAgent.speed = 0f;
    }
    public void StandUpEnd()
    {
        stateManager.navMeshAgent.speed = stateManager.chaseNavAgentSpeed;
        isStanding = true;
        isAttacking = false;

        stateManager.attackingHitBox.SetActive(true);
    }
}
