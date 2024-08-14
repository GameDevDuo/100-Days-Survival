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
    public Sprite toolSprite;
    public bool isCollecting;

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
                if (IsCheckTool(currentItem))
                {
                    currentItem.StartCollection();
                    isCollecting = true;
                }
                else
                {
                    Debug.Log("올바른 도구가 필요합니다!");
                }
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
                isCollecting = false;
            }
        }
        else
        {
            if (currentItem != null)
            {
                currentItem.StopCollection();
                currentItem = null;
                isCollecting = false;
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
                    isCollecting = false;
                }
            }
        }
        else
        {
            if (currentItem != null)
            {
                currentItem.StopCollection();
                currentItem = null;
                isCollecting = false;
            }
        }
    }

    bool IsCheckTool(ResourceItem item)
    {
        foreach (Sprite tool in item.itemData.ToolSprite)
        {
            if (tool == toolSprite)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * distance);
    }
}