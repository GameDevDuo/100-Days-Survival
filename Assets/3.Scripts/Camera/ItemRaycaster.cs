using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRaycaster : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask layerMask;
    private Camera playerCamera;  

    void Start()
    {
        playerCamera = GetComponent<Camera>();
        playerCamera = Camera.main;
    }

    void Update()
    {
        RaycastForItem();
    }

    void RaycastForItem()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                Debug.Log("2");
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