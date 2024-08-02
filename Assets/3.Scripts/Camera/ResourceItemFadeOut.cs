using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItemFadeOut : MonoBehaviour
{
    public float fadeDuration = 5.0f;
    private Material material;
    private Color color;
    private float timer;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        color = material.color;
        timer = fadeDuration;
    }

    public IEnumerator BeginFadeOut()
    {
        yield return new WaitForSeconds(Random.Range(12.5f, 17.5f));

        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;

            float a = timer / fadeDuration;
            Color newColor = color;
            newColor.a = a;
            material.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }
}