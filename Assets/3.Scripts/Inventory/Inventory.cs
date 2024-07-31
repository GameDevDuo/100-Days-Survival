using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField] private Slot[] slots;

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

    public void AddResourceItem(ResourceItem item)
    {
        string itemName = item.itemData.ItemName;
        Sprite sprite = item.itemData.Sprite;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemName == itemName)
            {
                slots[i].InsertItem();
                return;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].count == 0)
            {
                slots[i].InsertItem(sprite);
                return;
            }
        }
    }
}
