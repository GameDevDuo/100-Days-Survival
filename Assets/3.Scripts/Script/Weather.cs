using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weather : WeatherBase
{
    private List<WeatherData> weatherData;
    private List<GameObject> ableWeather;

    private void Start()
    {
        AbleWeatherList();
    }

    public override void AbleWeatherList()
    {
        foreach (var weather in weatherData)
        {
            if (UIManager.Instance.day <= weather.WeatherDate)
            {
                ableWeather.Add(weather.WeatherObject);
            }
        }
    }
}
