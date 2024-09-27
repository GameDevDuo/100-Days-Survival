using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private const int maxHealth = 100;
    private const int maxHunger = 100;
    private const int maxStamina = 100;
    private const float maxTemperature = 36.5f;
    private const int maxMentalState = 100;
    private const int maxThirst = 100;

    [SerializeField] private Image healthGauge;
    [SerializeField] private Image hungerGauge;
    [SerializeField] private Image staminaGauge;
    [SerializeField] private Image mentalGauge;
    [SerializeField] private Image thirstGauge;

    [SerializeField] private int curHealth = maxHealth;
    [SerializeField] private int curHunger = maxHunger;
    [SerializeField] private int curStamina = maxStamina;
    [SerializeField] private float curTemperature = maxTemperature;
    [SerializeField] private int curMentalState = maxMentalState;
    [SerializeField] private int curThirst = maxThirst;

    [SerializeField] private float hungerDecayRate = 1f;
    [SerializeField] private float thirstDecayRate = 1f;
    [SerializeField] private float staminaDecayRate = 0.5f;

    void Start()
    {
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
            curHunger -= Mathf.RoundToInt(hungerDecayRate * Time.deltaTime);
        }
        else
        {
            curHealth -= 1;
        }
    }

    private void DecreaseThirst()
    {
        if (curThirst > 0)
        {
            curThirst -= Mathf.RoundToInt(thirstDecayRate * Time.deltaTime);
        }
        else
        {
            curHealth -= 1;
        }
    }

    private void DecreaseStamina()
    {
        if (curStamina > 0)
        {
            curStamina -= Mathf.RoundToInt(staminaDecayRate * Time.deltaTime);
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
        hungerGauge.fillAmount = (float)curHunger / maxHunger;
        staminaGauge.fillAmount = (float)curStamina / maxStamina;
        mentalGauge.fillAmount = (float)curMentalState / maxMentalState;
        thirstGauge.fillAmount = (float)curThirst / maxThirst;
    }
}