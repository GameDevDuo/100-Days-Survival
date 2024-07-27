using UnityEngine;

public abstract class WeatherBase : MonoBehaviour, IOnOff
{

    public abstract void AbleWeatherList();

    public void OnOff(GameObject gameObject, bool value)
    {
        gameObject.SetActive(value);
    }
}
