using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Ingredient",menuName ="Custom Data/Ingredients/Ingredient")]
public class IngredientSO : BaseIngredientSO
{
    public Sprite ingredientIcon = null;
    public GameObject ingredientPrefab = null;
}
