using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : WeatherBase
{
    //폭풍우 - 0일부터 1일마다 15% 확률
    //폭염(가뭄) - 5일부터 1일마다 10% 확률
    //태풍 - 10일부터 3일마다 5% 확률
    //쓰나미 - 20일부터 3일마다 5% 확률
    //화산폭발 - 50일 확정 이후에 10일마다 3% 100일 확정
    //지진 - 50일부터 10일마다 3% 확률

    public static WeatherManager Instance;

    [SerializeField]
    private List<WeatherData> weatherData;
    private Dictionary<int, WeatherData> ableWeather = new Dictionary<int, WeatherData>();

    private int[] weatherIndex = new int[6];
    private int weatherHour;

    private int hour = 0;
    private int Hour
    {
        get
        {
            return hour;
        }
        set
        {
            if(hour != value && ableWeather.Count > 0)
            {
                WeatherTime(ref weatherHour);
                hour = value;
            }
            else
            {
                hour = 0;
            }
        }
    }
    private int second;

    private bool isGenerated = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        AbleWeatherList();
        GenerateWeather(ableWeather);

        for(int i = 0; i < weatherIndex.Length; i++)
        {
            weatherIndex[i] = 0;
        }
    }

    private void Update()
    {
        float gameTime = Time.deltaTime * 60;
        second = Mathf.FloorToInt(gameTime);
        Hour = (second / 3600) % 24;
    }

    private void WeatherTime(ref int weatherHour)
    {
        weatherHour -= 1;
    }
    
    public override void AbleWeatherList()
    {
        foreach (var weather in weatherData)
        {
            if (UIManager.Instance.Day <= weather.GenerateDate && !weather.IsAdded)
            {
                ableWeather.Add(weather.WeatherNum, weather);
                weather.IsAdded = true;
            }
        }

        GenerateWeather(ableWeather);
    }

    public void GenerateWeather(Dictionary<int, WeatherData> weather)
    {
        foreach(var finalWeather in weather)
        {
            if(finalWeather.Value.GenerateTerm == weatherIndex[finalWeather.Key - 1])
            {
                weatherIndex[finalWeather.Key - 1] = 0;
                if(Random.Range(0.00f, 1.00f) >= finalWeather.Value.WeatherPercent && !isGenerated)
                {
                    weatherHour = Random.Range(3, finalWeather.Value.GenerateTime);
                    GameObject gameObject = finalWeather.Value.WeatherObject;
                    Instantiate(gameObject, Camera.main.transform);
                    isGenerated = true;
                    if(weatherHour <= 0)
                    {
                        Destroy(gameObject);
                        isGenerated = false;
                    }
                }
            }
            else
            {
                weatherIndex[finalWeather.Key - 1]++;
            }
        }
    }
}