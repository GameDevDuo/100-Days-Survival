using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public ItemData itemData;
    private Rigidbody rb;
    private float currentCollectTime = 0.0f;
    private bool isBeingCollected = false;
    public float collectionTime = 0.0f;
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
            float progress = currentCollectTime / collectionTime;

            UIManager.Instance.UpdateGauge(progress);

            if (currentCollectTime >= collectionTime)
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
        return currentCollectTime / collectionTime;
    }

    private void FallDown()
    {
        rb = gameObject.AddComponent<Rigidbody>();

        Vector3 fallDirection = Camera.main.transform.forward;
        rb.AddForce(fallDirection * 1.25f, ForceMode.Impulse);
        StartCoroutine(IncreaseGravity());
    }

    private IEnumerator IncreaseGravity()
    {
        Vector3 original = Physics.gravity;
        Vector3 custom = new Vector3(original.x, original.y * 5, original.z);

        float duration = 7.5f;
        float time = 0f;

        while (time < duration)
        {
            rb.AddForce(custom * rb.mass * Time.fixedDeltaTime, ForceMode.Acceleration);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
