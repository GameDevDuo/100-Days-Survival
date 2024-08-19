using UnityEngine;

[CreateAssetMenu(fileName = "WeatherData", menuName = "ScriptableObject/WeatherData", order = int.MaxValue)]
public class WeatherData : ScriptableObject
{
    [SerializeField] private GameObject weatherObject;
    public GameObject WeatherObject { get { return weatherObject; } }

    [SerializeField] private int generateDate;
    public int GenerateDate { get { return generateDate; } }
}
