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
    public float AttackDistance { get { return attakDistance; } }

    [SerializeField] private float attakSpeed;
    public float AttackSpeed { get { return attakSpeed; } }

    [SerializeField] private float findRange;
    public float FindRange { get {  return findRange; } }

    [SerializeField] private Sprite[] rewardItem;
    public Sprite[] RewradItem { get { return rewardItem; } }
}