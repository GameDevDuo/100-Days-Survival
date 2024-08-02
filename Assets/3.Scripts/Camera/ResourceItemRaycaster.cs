using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceItemRaycaster : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask layerMask;
    private Camera playerCamera;
    private ResourceItem currentItem;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        RaycastResourceItem();

        if (Mouse.current.leftButton.isPressed)
        {
            if (currentItem != null && !currentItem.isCollectible)
            {
                currentItem.StartCollection();
            }
            else if (currentItem != null)
            {
                Inventory.Instance.AddResourceItem(currentItem);
                Rigidbody itemRigidbody = currentItem.GetComponent<Rigidbody>();
                currentItem.gameObject.layer = 0;
                if (currentItem.CompareTag("Tree") && itemRigidbody != null)
                {
                    itemRigidbody.isKinematic = false;
                    itemRigidbody.mass = 100f;
                    itemRigidbody.drag = 6f;
                    itemRigidbody.angularDrag = 2.5f;

                    if (currentItem.TryGetComponent(out ResourceItemFadeOut resourceItem))
                    {
                        StartCoroutine(resourceItem.BeginFadeOut());
                    }
                }
                else
                {
                    Destroy(currentItem.gameObject);
                }
            }
        }
        else
        {
            if (currentItem != null)
            {
                currentItem.StopCollection();
                currentItem = null;
            }
        }
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
                if (currentItem != item)
                {
                    if (currentItem != null)
                    {
                        currentItem.StopCollection();
                    }
                    currentItem = item;
                }
            }
            else
            {
                if (currentItem != null)
                {
                    currentItem.StopCollection();
                    currentItem = null;
                }
            }
        }
        else
        {
            if (currentItem != null)
            {
                currentItem.StopCollection();
                currentItem = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * distance);
    }
}