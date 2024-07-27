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
        gameTime += Time.deltaTime * 60f;

        totalSecond = Mathf.FloorToInt(gameTime);
        int hours = (totalSecond % 43200) / 3600;
        int minutes = (totalSecond % 43200) / 60;

        timeText.text = string.Format("{0:D2}시 : {1:D2}분", hours, minutes);
    }

    public void AddDate()
    {
        day = totalSecond / 43200;

        dateText.text = day + "일차";
    }
}
