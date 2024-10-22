using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "ScriptableObject/AnimalData", order = int.MaxValue)]
public class AnimalData : ScriptableObject
{
    [SerializeField] private string animalName;
    public string AnimalName {  get { return animalName; } }
    [SerializeField] private float maxHp;
    public float MaxHP { get { return maxHp; } }
    [SerializeField] private int damage;
    public int Damage { get { return damage; } }
    [SerializeField] private float attakDistance;
    public float AttackDistance { get { return attakDistance; } }
    [SerializeField] private float attackCoolTime;
    public float AttackCoolTime { get { return attackCoolTime; } }
    [SerializeField] private float findRange;
    public float FindRange { get {  return findRange; } }
}