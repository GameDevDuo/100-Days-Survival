using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField] private Slot[] slots;
    [SerializeField] private GameObject inventoryUI;

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

    public void ToggleInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
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
