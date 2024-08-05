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

    [SerializeField] private float attakDistance;
    public float AttakDistance { get { return attakDistance; } }

    [SerializeField] private float attakSpeed;
    public float AttakSpeed { get { return attakSpeed; } }

    [SerializeField] private float findRange;
    public float FindRange { get {  return findRange; } }

    [SerializeField] private GameObject[] rewardItem;
    public GameObject[] RewradItem { get { return rewardItem; } }
}