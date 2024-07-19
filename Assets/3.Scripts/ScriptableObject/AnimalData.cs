using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "AnimalData", menuName = "ScriptbleObject/AnimalData", order = int.MaxValue)]
public class AnimalData : ScriptableObject
{
    [SerializeField] private string animalName;
    public string AnimalName {  get { return animalName; } }

    [SerializeField] private float maxHp;
    public float MaxHP { get { return maxHp; } }

    [SerializeField] private float damage;
    public float Damage { get { return damage; } }

    [SerializeField] private GameObject[] rewardItem;
    public GameObject[] RewardItem { get { return rewardItem; } }
}
