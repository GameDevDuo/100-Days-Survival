using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/ItemData", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    public string ItemName { get { return itemName; } }
    [SerializeField] private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }
    [SerializeField] private float collectionTime;
    public float CollectionTime { get { return collectionTime; } }
}