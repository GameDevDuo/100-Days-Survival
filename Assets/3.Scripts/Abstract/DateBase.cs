using UnityEngine;

public abstract class DateBase : MonoBehaviour, ITime, IDay
{
    private const float RealSeconds = 1800f; //30minutes
    private float elapsedTime;

    protected int seconds;
    protected int minutes;
    protected int hours;
    protected int days;

    protected virtual void Update()
    {
        UpdateElapsedTime();
        UpdateGameTime();
        Debug.Log(seconds);
    }

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
        
    }

    public abstract void AddTime();
    public abstract void AddDate();
}
