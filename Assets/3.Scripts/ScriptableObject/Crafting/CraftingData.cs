using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingData", menuName = "ScriptableObject/CraftingData", order = int.MaxValue)]
public class CraftingData : ScriptableObject
{
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();
    public List<Recipe> Recipes { get { return recipes; } }
}

[System.Serializable]
public class Recipe
{
    public Sprite resultItem;
    public List<Ingredient> ingredients = new List<Ingredient>();
}

[System.Serializable]
public class Ingredient
{
    public Sprite itemSprite;
    public int count;
}