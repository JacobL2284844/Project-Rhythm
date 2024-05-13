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

    public MenuManager menuManager;

    public float lowHealthMusicThreshold = 40;
    private float musicValueBeforeHealthEffect;

    private Colouration colourationForStartDisolve;

    [Header("Player Health Regen")]
    public int regenAmountPerBeat = 5;//only takes place at stage 5
    public BeatClicker beatClicker;

    void Start()
    {
        currentHealth = maxHealth;

        if (gameObject.tag == "Enemy")
        {
            colourationForStartDisolve = GetComponent<Colouration>();
        }
    }
    public void TakeDamage(float amount)
    {
        float healthBeforAttack = currentHealth;

        if (maxHealth > (currentHealth -= amount))//on heal !> max
        {
            currentHealth -= amount;
            if (gameObject.tag == "Enemy")
            {
                //hit enemy sounds bad
                //AudioManager.instance.PLayOneShot(AudioManager.instance.enemyTakeDamage, transform.position);
            }
            else if (gameObject.tag == "Player")
            {
                AudioManager.instance.PLayOneShot(AudioManager.instance.takeDamage, transform.position);

                float fillAmount_A = healthBeforAttack / maxHealth;//apply
                float fillAmount_B = currentHealth / maxHealth;

                if (amount > 0f)
                {
                    //random hit react
                    playerHitEffect.Play();
                    playerAnimator.runtimeAnimatorController = playerHitReaction[Random.Range(0, playerHitReaction.Length)];
                    playerAnimator.SetTrigger("HitReact");
                }

                //low health music logic
                if (currentHealth <= lowHealthMusicThreshold)//low health music
                {
                    beatClicker.bgmEventInstance.getParameterByName("Health", out float musicHealthValue);
                    musicValueBeforeHealthEffect = musicHealthValue;

                    beatClicker.SetMusicParamaterCombat(0.5f);
                }
                else if(currentHealth > lowHealthMusicThreshold)//reset if heal
                {
                    beatClicker.SetMusicParamaterCombat(musicValueBeforeHealthEffect);
                }

                StartCoroutine(LowerHealthBar(fillAmount_A, fillAmount_B));
            }
        }
        if (currentHealth <= 0f && gameObject.tag == "Enemy")
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

            colourationForStartDisolve.ApplyDestructionToElements();
            Destroy(gameObject, 0.5f);
        }
        if (gameObject.tag == "Player")
        {
            menuManager.ShowDeathMenu();
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

            if (healthBarL.fillAmount <= 0f)
            {
                Die();
            }

            yield return null;
        }
    }

    public void TryHealPlayerStage5()
    {
        if (beatClicker.currentStage == BeatClicker.Stage.Stage5)
        {
            TakeDamage(-regenAmountPerBeat);
            //beatClicker.SetMusicParamaterCombat(10);// reset after george
        }
    }
}