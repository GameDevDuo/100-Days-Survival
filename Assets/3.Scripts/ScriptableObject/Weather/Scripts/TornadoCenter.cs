using System.Collections.Generic;
using UnityEngine;

public class TornadoCenter : MonoBehaviour
{
    private Collider[] colliders;
    private LayerMask layer;

    private float radius = 50f;
    private float speed = 2f;

    private void Start()
    {
        layer = (1 << 7) | (1 << 9);
    }

    private void Update()
    {
        if(GameManager.Instance.CheckForObject(out colliders, transform.position, radius, layer))
        {
            Debug.Log("Check!");
            foreach(Collider col in colliders)
            {
                Debug.Log("Pulling!");
                col.transform.position = Vector3.Lerp(col.transform.position, transform.position, speed);
            }
        }
    }
}
