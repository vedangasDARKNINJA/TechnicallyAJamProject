using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeChoicePanel : MonoBehaviour
{
    public static event Action<RecipeSO> OnRecipeSelected;

    [SerializeField]
    private CanvasGroup m_CanvasGroup = null;

    [SerializeField]
    private Button m_CloseButton = null;

    [SerializeField]
    private Transform m_RecipeCardParent = null;

    [SerializeField]
    private GameObject m_RecipeCardPrefab = null;

    [SerializeField]
    private List<RecipeCard> m_RecipeCards = new List<RecipeCard>();

    private void OnEnable()
    {
        CauldronManager.OnChooseRecipe += OnChooseRecipe;
        m_CloseButton?.onClick.AddListener(ClosePrompt);
        RecipeCard.onRecipeSelected += OnRecipeCardSelected;
    }

    private void OnDisable()
    {
        CauldronManager.OnChooseRecipe -= OnChooseRecipe;
        m_CloseButton?.onClick.RemoveListener(ClosePrompt);
        RecipeCard.onRecipeSelected -= OnRecipeCardSelected;
    }

    private void OnChooseRecipe()
    {
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.interactable = false;

        RefreshCards();

        transform.DOScale(1.0f, 0.5f)
            .From(0.0f)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                m_CanvasGroup.interactable = true;
                m_CanvasGroup.blocksRaycasts = true;
            });
    }

    private void ClosePrompt()
    {
        CloseAnimation();
        OnRecipeSelected?.Invoke(null);
    }

    private void RefreshCards()
    {
        int i = 0;
        foreach (var recipe in RecipeDataManager.instance.recipes)
        {
            if (i < m_RecipeCards.Count)
            {
                m_RecipeCards[i].SetData(recipe);
            }
            else
            {
                if (m_RecipeCardPrefab != null && Instantiate(m_RecipeCardPrefab, m_RecipeCardParent).TryGetComponent(out RecipeCard card))
                {
                    card.SetData(recipe);
                    m_RecipeCards.Add(card);
                }
            }

            i++;
        }
    }

    void CloseAnimation()
    {
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.interactable = true;

        transform.DOScale(0.0f, 0.2f)
            .From(1.0f)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                m_CanvasGroup.interactable = false;
            });
    }

    void OnRecipeCardSelected(RecipeSO recipe)
    {
        CloseAnimation();
        OnRecipeSelected?.Invoke(recipe);
    }
}
