using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeStatsUI : MonoBehaviour
{
    public static event Action OnRecipeCancelled;

    [SerializeField]
    private RectTransform m_IngredientParent;

    [SerializeField]
    private GameObject m_IngredientPrefab;

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [SerializeField]
    private Button m_CancelButton;

    private Dictionary<int, RecipeIngredientUI> m_IngredientsMap = new Dictionary<int, RecipeIngredientUI>();

    private void OnEnable()
    {
        CauldronManager.OnRecipeIngredientAdded += OnRecipeIngredientAdded;
        RecipeChoicePanel.OnRecipeSelected += OnRecipeSelected;
        m_CancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnDisable()
    {
        CauldronManager.OnRecipeIngredientAdded -= OnRecipeIngredientAdded;
        RecipeChoicePanel.OnRecipeSelected -= OnRecipeSelected;
        m_CancelButton.onClick.RemoveListener(OnCancelButtonClicked);
    }

    private void OnDestroy()
    {
        CauldronManager.OnRecipeIngredientAdded -= OnRecipeIngredientAdded;
        RecipeChoicePanel.OnRecipeSelected -= OnRecipeSelected;
        m_CancelButton.onClick.RemoveListener(OnCancelButtonClicked);
    }

    void OnCancelButtonClicked()
    {
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
        DeactivateChildren();
        OnRecipeCancelled?.Invoke();
    }

    private void OnRecipeSelected(RecipeSO recipe)
    {
        m_IngredientsMap.Clear();
        if (recipe == null)
        {
            return;
        }
        m_CanvasGroup.interactable = true;
        m_CanvasGroup.blocksRaycasts = true;
        DeactivateChildren();

        for (int i = 0; i < recipe.RecipeData.Count; ++i)
        {
            var recipeData = recipe.RecipeData[i];
            var ingredient = recipeData.ingredient;
            AddIngredient(i, ingredient.identifier.id, ingredient.ingredientIcon, recipeData.quantity);
        }
    }

    private void AddIngredient(int index,int id, Sprite ingredientIcon, uint quantity)
    {
        GameObject go;
        if (index >= m_IngredientParent.childCount)
        {
            go = Instantiate(m_IngredientPrefab, m_IngredientParent);
        }
        else
        {
            go = m_IngredientParent.GetChild(index).gameObject;
            go.SetActive(true);
        }

        if(go.TryGetComponent(out RecipeIngredientUI ingredientUI))
        {
            m_IngredientsMap.Add(id, ingredientUI);
            ingredientUI.SetData(ingredientIcon, quantity);
        }
    }

    private void DeactivateChildren()
    {
        int count = m_IngredientParent.childCount;
        for (int i = 0; i < m_IngredientParent.childCount; ++i)
        {
            m_IngredientParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnRecipeIngredientAdded(Dictionary<int, int> ingredientStatus)
    {
        foreach (KeyValuePair<int, RecipeIngredientUI> ingredientData in m_IngredientsMap)
        {
            int key = ingredientData.Key;
            if (ingredientStatus.ContainsKey(key))
            {
                m_IngredientsMap[key].SetQuantity(ingredientStatus[key]);
            }
            else
            {
                m_IngredientsMap[key].SetQuantity(0);
            }

        }
    }
}
