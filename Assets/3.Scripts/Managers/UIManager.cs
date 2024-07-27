using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITime, IDay
{
    public static UIManager Instance;

    [SerializeField] private Image circularGauge;

    public Text timeText;
    public Text dateText;

    private int totalSecond;
    public int day;

    private float gameTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        gameTime += Time.deltaTime * 30f;

        totalSecond = Mathf.FloorToInt(gameTime);
        int hours = (totalSecond % 86400) / 3600;
        int minutes = (totalSecond % 86400) / 60;

        timeText.text = string.Format("{0:D2}:{1:D2} AM", hours, minutes);
    }

    public void AddDate()
    {
        int day = totalSecond / 86400;

        dateText.text = string.Format("Day {0}", day + 1);
    }

}