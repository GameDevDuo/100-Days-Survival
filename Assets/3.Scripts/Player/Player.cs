using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private const int maxHealth = 100;
    private const int maxHunger = 100;
    private const int maxStamina = 100;
    private const int maxMentalState = 100;
    private const int maxThirst = 100;

    [SerializeField] private Image healthGauge;
    [SerializeField] private Image hungerGauge;
    [SerializeField] private Image staminaGauge;
    [SerializeField] private Image mentalGauge;
    [SerializeField] private Image thirstGauge;
    [SerializeField] private Image damageFeedback;

    [SerializeField] private int curHealth = maxHealth;
    [SerializeField] private float curHunger = maxHunger;
    [SerializeField] private int curMentalState = maxMentalState;
    [SerializeField] private float curThirst = maxThirst;

    [SerializeField] private float hungerDecayRate = 1f;
    [SerializeField] private float thirstDecayRate = 1f;
    [SerializeField] private float staminaDecayRate = 0.5f;

    private PlayerController playerController;

    public float curStamina = maxStamina;
    public bool isRun;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        damageFeedback.enabled = false;
        UpdateGauges();
    }

    void Update()
    {
        DecreaseHunger();
        DecreaseThirst();
        DecreaseStamina();

        CheckPlayerDeath();
        UpdateGauges();
    }

    private void DecreaseHunger()
    {
        if (curHunger > 0)
        {
            curHunger -= hungerDecayRate * Time.deltaTime;

            if (curHunger < 0)
            {
                curHunger = 0;
            }
        }
        else
        {
            TakeDamage(1);
        }
    }

    private void DecreaseThirst()
    {
        if (curThirst > 0)
        {
            curThirst -= thirstDecayRate * Time.deltaTime;

            if (curThirst < 0)
            {
                curThirst = 0;
            }
        }
        else
        {
            TakeDamage(1);
        }
    }

    private void DecreaseStamina()
    {
        if (isRun)
        {
            curStamina -= staminaDecayRate * Time.deltaTime;

            if (curStamina <= 0)
            {
                curStamina = 0;
                isRun = false;
                playerController.isRunning = false;
            }
        }
        else
        {
            if (curStamina < maxStamina)
            {
                curStamina += (staminaDecayRate / 2) * Time.deltaTime;
            }
        }
    }

    private void CheckPlayerDeath()
    {
        if (curHealth <= 0)
        {
            Debug.Log("Player Dead!");
        }
    }

    private void UpdateGauges()
    {
        healthGauge.fillAmount = (float)curHealth / maxHealth;
        hungerGauge.fillAmount = curHunger / maxHunger;
        staminaGauge.fillAmount = curStamina / maxStamina;
        mentalGauge.fillAmount = (float)curMentalState / maxMentalState;
        thirstGauge.fillAmount = curThirst / maxThirst;
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;

        if (curHealth < 0)
        {
            curHealth = 0;
        }
        StartCoroutine(ShowDamageFeedback());

        CheckPlayerDeath();
        UpdateGauges();
    }

    private IEnumerator ShowDamageFeedback()
    {
        damageFeedback.enabled = true;

        Color color = damageFeedback.color;

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * 2f;
            color.a = Mathf.Clamp01(alpha);
            damageFeedback.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * 2f;
            color.a = Mathf.Clamp01(alpha);
            damageFeedback.color = color;
            yield return null;
        }
        damageFeedback.enabled = false;
    }
}