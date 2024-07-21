using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public ItemData ItemData { get { return itemData; } }
}