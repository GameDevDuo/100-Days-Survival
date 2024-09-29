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
    [SerializeField] private float curHunger = maxHunger;
    [SerializeField] private float curTemperature = maxTemperature;
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
            curHealth -= 1;
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
            curHealth -= 1;
        }
    }

    private void DecreaseStamina()
    {
        if (isRun)
        {
            if (curStamina > 0)
            {
                curStamina -= staminaDecayRate * Time.deltaTime;

                if (curStamina <= 0)
                {
                    curStamina = 0;
                    isRun = false;
                    playerController.isRunning = false;
                }
            }
        }
        else
        {
            if (curStamina < maxStamina)
            {
                curStamina += staminaDecayRate * Time.deltaTime;
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
}