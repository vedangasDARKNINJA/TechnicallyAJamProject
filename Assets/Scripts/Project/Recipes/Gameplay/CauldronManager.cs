using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CauldronState
{
    Available,
    Picked,
    WaitingIngredients,
    CookingMonster,
}

public class CauldronManager : MonoBehaviour
{
    public static event Action OnChooseRecipe;
    public static event Action<Dictionary<int, uint>> OnRecipeIngredientAdded;

    private InteractionPromptComponent m_InteractionPrompt;

    [SerializeField]
    private RecipeSO m_CurrentRecipe;

    private Dictionary<int, uint> m_RecipeStats = new Dictionary<int, uint>();

    private CauldronState m_CurrentState = CauldronState.Available;

    private void Awake()
    {
        GameInputActions inputActions = InputSystemController.instance.gameInputActions;
        inputActions.Player.Recipes.performed += OnRecipesInputRecieved;

        OnRecipeSelected(m_CurrentRecipe);
        PrintRecipeStats();
    }

    private void OnRecipesInputRecieved(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (m_CurrentState != CauldronState.Available)
        {
            return;
        }

        OnChooseRecipe?.Invoke();
    }

    public void OnRecipeSelected(RecipeSO recipe)
    {
        m_RecipeStats.Clear();
        foreach (var data in recipe.RecipeData)
        {
            m_RecipeStats.Add(data.ingredient.identifier.id, data.quantity);
        }
    }

    public void AddIngredient(GameObject ingredientObject)
    {
        IngredientObject ingredientComponent = ingredientObject.GetComponent<IngredientObject>();
        if (ingredientObject)
        {
            int id = ingredientComponent.identifier.id;
            print($"Currently SUbmitted: Id: {id}");

            if (m_RecipeStats.ContainsKey(id))
            {
                m_RecipeStats[id]--;
                if (m_RecipeStats[id] == 0)
                {
                    m_RecipeStats.Remove(id);
                }
                PrintRecipeStats();
                OnRecipeIngredientAdded?.Invoke(m_RecipeStats);
            }
        }
    }

    public void SetState(CauldronState state)
    {
        if (m_CurrentState != state)
        {

        }
    }

    private void PrintRecipeStats()
    {
        print($"======= {m_CurrentRecipe.name} =======");
        if (m_RecipeStats.Count > 0)
        {
            foreach (var data in m_RecipeStats)
            {
                print($"Id: {data.Key}, Remaining: {data.Value}");
            }
        }
        else
        {
            print($"Recipe Complete! Wait till cooking is finished!");
        }
        print($"==============================");
    }
}
