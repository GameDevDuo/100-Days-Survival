using UnityEngine;
using UnityEngine.UI;

public class GameDate : DateBase
{
    protected override void Update()
    {
        base.Update();
        AddDate();
        AddTime();
    }

    public override void AddDate()
    {
        UIManager.Instance.dateText.text = days + "일차";
    }

    public override void AddTime()
    {
        UIManager.Instance.timeText.text = string.Format("{0:D2}시 : {1:D2}분", hours, minutes);
    }
}
