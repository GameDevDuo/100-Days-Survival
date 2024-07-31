using System.Collections;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public ItemData itemData;
    private Rigidbody rb;
    private float currentCollectTime = 0.0f;
    private bool isBeingCollected = false;
    public bool isCollectible = false;

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
        isCollectible = true;
        currentCollectTime = 0.0f;
        UIManager.Instance.ShowGauge(false);
        UIManager.Instance.UpdateGauge(0);

        if (CompareTag("Tree"))
        {
            FallDown();
        }
    }

    public float GetCurrentProgress()
    {
        return currentCollectTime / itemData.CollectionTime;
    }

    private void FallDown()
    {
        rb = gameObject.AddComponent<Rigidbody>();

        Vector3 fallDirection = Camera.main.transform.forward;
        rb.AddForce(fallDirection * 1.25f, ForceMode.Impulse);
    }
}