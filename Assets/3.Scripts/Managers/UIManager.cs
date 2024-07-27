using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITime, IDay
{
    public static UIManager Instance;

    [SerializeField] private Image circularGauge;

    public Text timeText;
    public Text dateText;

    private int totalSecond;

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

        timeText.text = string.Format("{0:D2}시 : {1:D2}분", hours, minutes);
    }

    public void AddDate()
    {
        int day = totalSecond / 86400;

        dateText.text = day + "일차";
    }
}
