using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public float currentHealth = 0f;
    public float maxHealth = 100f;
    public float maxHealth_AfterUpgrades = 100f;
    public Image healthBar;

    public bool isEnemy = true;

    public ParticleSystem playerHitEffect;
    public float healthSmoothDecreaseDuration;
    //public GameObject deathMenu;
    void Start()
    {
        if (gameObject.tag == "Player")
        {
            float fillAmount_A = currentHealth / maxHealth;
            float fillAmount_B = currentHealth / maxHealth;
            StartCoroutine(LowerHealthBar(fillAmount_A, fillAmount_B));

            if (maxHealth >= maxHealth_AfterUpgrades)
            {
                maxHealth = maxHealth_AfterUpgrades;
            }
        }
        else
        {
            currentHealth = maxHealth;
        }
    }
    public void TakeDamage(float amount)
    {
        float healthBeforAttack = currentHealth;

        currentHealth -= amount;
        if (gameObject.tag == "NPC")
        {
            NPCStateManager stateManager = GetComponent<NPCStateManager>();

            if (stateManager != null)
            {
                if (stateManager.currantStateStr != "Chase" || stateManager.currantStateStr != "Attack")
                {
                    if (stateManager.isStandardEnemy)
                    {
                        stateManager.SetState(stateManager.chaseState);
                    }
                }
            }
        }
        else if (gameObject.tag == "Player")
        {
            float fillAmount_A = healthBeforAttack / maxHealth;
            float fillAmount_B = currentHealth / maxHealth;

            playerHitEffect.Play();

            StartCoroutine(LowerHealthBar(fillAmount_A, fillAmount_B));
        }
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        if (gameObject.tag == "NPC")
        {
            Vector3 pos = new Vector3(transform.position.x, 0.02f, transform.position.z);
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