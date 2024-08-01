using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "ScriptableObject/AnimalData", order = int.MaxValue)]
public class AnimalData : ScriptableObject
{
    [SerializeField] private string animalName;
    public string AnimalName {  get { return animalName; } }

    [SerializeField] private float maxHp;
    public float MaxHP { get { return maxHp; } }

    [SerializeField] private float damage;
    public float Damage { get { return damage; } }

    [SerializeField] private GameObject[] rewardItem;
    public GameObject[] GetGameObjects() => rewardItem;
}