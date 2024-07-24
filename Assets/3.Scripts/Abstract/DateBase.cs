using UnityEngine;

public abstract class DateBase : MonoBehaviour, ITime, IDay
{
    private const float RealSeconds = 1800f; //30minutes
    private float elapsedTime;

    private int seconds;
    private int minutes;
    private int hours;
    private int days;

    private void UpdateElapsedTime()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= RealSeconds)
        {
            AddDate();
            elapsedTime -= RealSeconds;
        }
    }

    private void UpdateGameTime()
    {
        float gameTime = 86400f / RealSeconds;
        float gameSecond = elapsedTime / gameTime;

        seconds = (int)gameSecond % 60;
        minutes = (int)(gameSecond % 60) % 60;
        hours = (int)(gameSecond % 3600) % 24;
    }
    public abstract void SetTime();
    public abstract void AddTime();
    public abstract void AddDate();
}
