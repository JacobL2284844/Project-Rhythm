using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public float currentHealth = 0f;
    public float maxHealth = 100f;
    public Image healthBarR;
    public Image healthBarL;

    public bool isEnemy = true;

    public Animator playerAnimator;
    public AnimatorOverrideController[] playerHitReaction;
    public ParticleSystem playerHitEffect;
    public float healthSmoothDecreaseDuration;
    //public GameObject deathMenu;

    [Header("Player Health Regen")]
    public int regenAmountPerBeat = 5;//only takes place at stage 5
    public BeatClicker beatClicker;
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        float healthBeforAttack = currentHealth;

        if (maxHealth > (currentHealth -= amount))//on heal !> max
        {
            currentHealth -= amount;
            if (gameObject.tag == "Enemy")
            {
                NPCStateManager stateManager = GetComponent<NPCStateManager>();

                //if (stateManager != null)
                //{
                //    if (stateManager.currantStateStr != "Chase" || stateManager.currantStateStr != "Attack")
                //    {
                //        if (stateManager.isStandardEnemy)
                //        {
                //            stateManager.SetState(stateManager.chaseState);
                //        }
                //    }
                //}
            }
            else if (gameObject.tag == "Player")
            {
                float fillAmount_A = healthBeforAttack / maxHealth;//apply
                float fillAmount_B = currentHealth / maxHealth;

                if (amount > 0f)
                {
                    //random hit react
                    playerHitEffect.Play();
                    playerAnimator.runtimeAnimatorController = playerHitReaction[Random.Range(0, playerHitReaction.Length)];
                    playerAnimator.SetTrigger("HitReact");
                }

                StartCoroutine(LowerHealthBar(fillAmount_A, fillAmount_B));
            }
        }
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        if (gameObject.tag == "Enemy")
        {
            NPCStateManager stateManager = GetComponent<NPCStateManager>();
            stateManager.enemyMaster.enemys.Remove(stateManager);
            stateManager.mySpawner.myActiveEnemies.Remove(gameObject);

            Destroy(gameObject);
        }
        if (gameObject.tag == "Player")
        {

        }
    }

    IEnumerator LowerHealthBar(float startAmount, float endAmount)
    {
        float startTime = Time.time;

        float elapsedTime = 0f;

        while (elapsedTime < healthSmoothDecreaseDuration)
        {
            elapsedTime = Time.time - startTime;
            float t = elapsedTime / healthSmoothDecreaseDuration;

            float currentValue = Mathf.Lerp(startAmount, endAmount, t);

            healthBarR.fillAmount = currentValue;
            healthBarL.fillAmount = currentValue;

            yield return null;
        }
    }

    public void TryHealPlayerStage5()
    {
        if (beatClicker.currentStage == BeatClicker.Stage.Stage5)
        {
            TakeDamage(-regenAmountPerBeat);
        }
    }
}