using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeData
{
    public IngredientSO ingredient = null;
    public uint quantity = 1;
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Custom Data/Recipes/Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<RecipeData> RecipeData = new List<RecipeData>();

    public MonsterSO outcomeMonster = null;
}
