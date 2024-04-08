using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public float currentHealth = 0f;
    public float maxHealth = 100f;
    public Image healthBar;

    public bool isEnemy = true;

    public Animator playerAnimator;
    public ParticleSystem playerHitEffect;
    public float healthSmoothDecreaseDuration;
    //public GameObject deathMenu;
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        float healthBeforAttack = currentHealth;

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
            //float fillAmount_A = healthBeforAttack / maxHealth;
            //float fillAmount_B = currentHealth / maxHealth;

            playerHitEffect.Play();
            playerAnimator.SetTrigger("HitReact");
             //StartCoroutine(LowerHealthBar(fillAmount_A, fillAmount_B));
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

            healthBar.fillAmount = currentValue;

            yield return null;
        }
    }
}