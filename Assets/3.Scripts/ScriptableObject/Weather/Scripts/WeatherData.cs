using UnityEngine;

[CreateAssetMenu(fileName = "WeatherData", menuName = "ScriptableObject/WeatherData", order = int.MaxValue)]
public class WeatherData : ScriptableObject
{
    [SerializeField] private GameObject weatherObject;
    public GameObject WeatherObject { get { return weatherObject; } }

    [SerializeField] private float weatherPercent;
    public float WeatherPercent { get { return weatherPercent; } }

    [SerializeField] private int generateDate;
    public int GenerateDate { get { return generateDate; } }

    [SerializeField] private int generateTerm;
    public int GenerateTerm { get { return generateTerm; } set { generateTerm = value; } }

    [SerializeField] private int weatherNum;
    public int WeatherNum { get { return weatherNum; } }
}
