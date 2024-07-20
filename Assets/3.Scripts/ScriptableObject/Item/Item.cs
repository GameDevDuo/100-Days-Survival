using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public ItemData ItemData { get { return itemData; } }
}
