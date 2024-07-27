using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public ItemData itemData;
    private float currentCollectTime = 0.0f;
    private bool isBeingCollected = false;

    public void StartCollection()
    {
        if (!isBeingCollected)
        {
            isBeingCollected = true;
            currentCollectTime = 0.0f;
            UIManager.Instance.ShowGauge(true);
        }
    }

    public void StopCollection()
    {
        if (isBeingCollected)
        {
            isBeingCollected = false;
            currentCollectTime = 0.0f;
            UIManager.Instance.ShowGauge(false);
            UIManager.Instance.UpdateGauge(0);
        }
    }

    void Update()
    {
        if (isBeingCollected)
        {
            currentCollectTime += Time.deltaTime;
            float progress = currentCollectTime / itemData.CollectionTime;

            UIManager.Instance.UpdateGauge(progress);

            if (currentCollectTime >= itemData.CollectionTime)
            {
                FinishCollection();
            }
        }
    }

    void FinishCollection()
    {
        isBeingCollected = false;
        currentCollectTime = 0.0f;
        UIManager.Instance.ShowGauge(false);
        UIManager.Instance.UpdateGauge(0);
    }

    public float GetCurrentProgress()
    {
        return currentCollectTime / itemData.CollectionTime;
    }
}
