using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient ID", menuName = "Custom Data/Ingredients/Ingredient ID")]
public class IngredientIdentifierSO : ScriptableObject
{
    public int id = 0;
    public string ingredientName;

    [MenuItem("Tools/AutoAssignIngredientIDs")]
    private static void AutoAssignIDs()
    {
        int id = 0;
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(IngredientIdentifierSO).Name);  //FindAssets uses tags check documentation for more info
        IngredientIdentifierSO[] assets = new IngredientIdentifierSO[guids.Length];
        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<IngredientIdentifierSO>(path);
            assets[i].id = ++id;
        }
        Debug.Log("Assigned Ids successfuly!");
    }
}
