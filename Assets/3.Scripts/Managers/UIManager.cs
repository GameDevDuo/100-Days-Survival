using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITime, IDay
{
    public static UIManager Instance;
    [SerializeField] private Image circularGauge;
    [SerializeField] private GameObject sunLight;
    public Text timeText;
    public Text dateText;
    private int second;
    public int day;
    private int hours;
    private int minutes;
    private float gameTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            gameTime = 12 * 3600;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        AddTime();
        AddDate();
        RotateSunLight();
    }

    public void ShowGauge(bool show)
    {
        circularGauge.gameObject.SetActive(show);
    }

    public void UpdateGauge(float progress)
    {
        circularGauge.fillAmount = progress;
    }

    public void AddTime()
    {
        gameTime += Time.deltaTime * 60f;

        second = Mathf.FloorToInt(gameTime);
        hours = (second / 3600) % 24;
        minutes = (second / 60) % 60;

        string time = hours >= 12 ? "PM" : "AM";
        int total = hours % 12;
        total = total == 0 ? 12 : total;

        timeText.text = string.Format("{0:D2}:{1:D2} {2}", total, minutes, time);
    }

    public void AddDate()
    {
        day = second / 86400;

        dateText.text = string.Format("DAY {0}", day + 1);
    }

    private void RotateSunLight()
    {
        float time = (hours * 60f + minutes) / 1440f;
        float angle = time * 360f;

        sunLight.transform.rotation = Quaternion.Euler(new Vector3(angle - 90, 170, 0));
    }
}