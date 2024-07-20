using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptbleObject/ItemData", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    public string ItemName { get { return itemName; } }
}
