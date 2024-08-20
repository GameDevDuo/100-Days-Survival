using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : WeatherBase
{
    public static WeatherManager Instance;

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
            if (UIManager.Instance.Day <= weather.GenerateDate)
            {
                ableWeather.Add(weather.WeatherObject);
            }
        }
    }
}
