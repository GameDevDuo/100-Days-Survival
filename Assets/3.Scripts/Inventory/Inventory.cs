using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField] private Slot[] slots;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    private FirstPersonCamera firstPersonCamera;
    private ResourceItemRaycaster resourceItemRaycaster;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        firstPersonCamera = player.transform.GetChild(0).GetComponent<FirstPersonCamera>();
        resourceItemRaycaster = player.transform.GetChild(0).GetComponent<ResourceItemRaycaster>();
    }

    public void ToggleInventory()
    {
        if (resourceItemRaycaster.isCollecting)
        {
            return;
        }

        bool isActive = !inventoryUI.activeSelf;
        inventoryUI.SetActive(isActive);

        playerController.enabled = !isActive;
        firstPersonCamera.enabled = !isActive;
        resourceItemRaycaster.enabled = !isActive;

        Cursor.visible = isActive;
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void AddResourceItem(ResourceItem item)
    {
        string itemName = item.itemData.ItemName;
        Sprite sprite = item.itemData.Sprite;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemName == itemName && slots[i].count < 64)
            {
                slots[i].InsertItem();
                return;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].count == 0)
            {
                slots[i].InsertItem(itemName, sprite);
                return;
            }
        }
    }
}