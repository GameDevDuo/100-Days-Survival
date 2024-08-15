using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildHandler : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private LayerMask layerMask;
    private Camera playerCamera;
    private ResourceItemRaycaster resourceItemRaycaster;
    private Inventory inventory;
    private GameObject hologram;

    void Start()
    {
        playerCamera = Camera.main;

        resourceItemRaycaster = GetComponent<ResourceItemRaycaster>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
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

                    if (itemData != null && itemData.IsBuildable)
                    {
                        if (hologram == null || hologram.name != sprite.name)
                        {
                            if (hologram != null)
                            {
                                Destroy(hologram);
                            }

                            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Map/Build/{sprite.name}");
                            if (prefab != null)
                            {
                                hologram = Instantiate(prefab, hit.point, Quaternion.identity);
                                hologram.name = sprite.name;
                                SetHologramMaterial(hologram);
                            }
                        }
                        if (hologram != null)
                        {
                            hologram.transform.position = hit.point;
                            hologram.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                            hologram.SetActive(true);

                            if (Mouse.current.rightButton.wasPressedThisFrame)
                            {
                                Instantiate(Resources.Load<GameObject>($"Prefabs/Map/Build/{sprite.name}"), hit.point, hologram.transform.rotation);
                                inventory.hotbarSlots[inventory.selectedSlot].transform.GetChild(0).GetComponent<Slot>().RemoveItem();
                                Destroy(hologram);
                            }
                            return;
                        }
                    }
                }
            }
        }
        if (hologram != null)
        {
            hologram.SetActive(false);
        }
    }

    private void SetHologramMaterial(GameObject hologram)
    {
        Renderer[] render = hologram.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in render)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.color = new Color(0, 1, 0, 0.5f); 
            material.SetFloat("_Mode", 3); 
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            renderer.material = material;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * distance);
    }
}