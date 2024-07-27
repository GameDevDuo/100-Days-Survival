using UnityEngine;

[CreateAssetMenu(fileName = "WeatherData", menuName = "ScriptableObject/WeatherData", order = int.MaxValue)]
public class WeatherData : ScriptableObject
{
    [SerializeField] private GameObject weatherPre;
    public GameObject WeatherPre {  get { return weatherPre; } }

    [SerializeField] private int weatherDate;
    public int WeatherDate { get { return weatherDate; } }
}
