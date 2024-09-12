using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private Color originalColor;
    private void Start()
    {
        originalColor = UIManager.Instance.sunLight.GetComponent<Light>().color;
        UIManager.Instance.sunLight.GetComponent<Light>().color = this.GetComponent<Light>().color;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        UIManager.Instance.sunLight.GetComponent<Light>().color = originalColor;
    }
}