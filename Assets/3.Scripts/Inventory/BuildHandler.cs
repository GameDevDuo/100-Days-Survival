using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHandler : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask layerMask;
    private Camera playerCamera;
    private ResourceItemRaycaster resourceItemRaycaster;
    private GameObject go;

    void Start()
    {
        playerCamera = Camera.main;

        resourceItemRaycaster = GetComponent<ResourceItemRaycaster>();
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            if (hit.collider != null)
            {
                Sprite sprite = resourceItemRaycaster.toolSprite;
                if (sprite != null)
                {
                    ItemData itemData = Resources.Load<ItemData>($"Prefabs/ItemData/{sprite.name}");

                    if (itemData.IsBuildable)
                    {
                        if (go != null)
                        {
                            go.transform.position = hit.point;
                            return;
                        }
                        go = Instantiate(Resources.Load<GameObject>("Prefabs/Map/Build/Bonfire"), hit.point, Quaternion.identity);
                    }
                    else
                    {
                        Destroy(go);
                    }
                }
                else
                {
                    Destroy(go);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * distance);
    }
}