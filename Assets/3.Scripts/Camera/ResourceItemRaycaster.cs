using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItemRaycaster : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask layerMask;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        RaycastResourceItem();
    }

    void RaycastResourceItem()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            ResourceItem item = hit.collider.GetComponent<ResourceItem>();
            if (item != null)
            {
                string itemDataName = item.ItemData.ItemName;
                Debug.Log(itemDataName);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * distance);
    }
}