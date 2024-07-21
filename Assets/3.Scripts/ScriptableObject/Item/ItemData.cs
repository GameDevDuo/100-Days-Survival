using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptbleObject/ItemData", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    public string ItemName { get { return itemName; } }
    [SerializeField] private float collectionTime;
    public float CollectionTime { get { return collectionTime; } }
}