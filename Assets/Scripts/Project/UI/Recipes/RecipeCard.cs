using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCard : MonoBehaviour
{
    public delegate void OnRecipeSelected(RecipeSO recipe);

    public static event OnRecipeSelected onRecipeSelected;

    [SerializeField]
    private Image m_MonsterIcon;

    [SerializeField]
    private TextMeshProUGUI m_MonsterName = null;

    [SerializeField]
    private RecipeStatRow m_StatRowPrefab = null;

    [SerializeField]
    private Transform m_StatRowParent = null;

    [SerializeField]
    private Button m_SelectButton = null;

    private RecipeSO m_CurrentRecipe = null;
    
    private void OnEnable()
    {
        m_SelectButton?.onClick.AddListener(() => onRecipeSelected?.Invoke(m_CurrentRecipe));
    }

    private void OnDisable()
    {
        m_SelectButton?.onClick.RemoveAllListeners();
    }

    public void SetData(RecipeSO recipe)
    {
        m_CurrentRecipe = recipe;
        int i = 0;
        while(i<m_StatRowParent.childCount)
        {
            m_StatRowParent.GetChild(i).gameObject.SetActive(false);
            i++;
        }

        m_MonsterIcon.sprite = recipe.outcomeMonster.monsterIcon;
        m_MonsterName.text = recipe.outcomeMonster.identifier.ingredientName;

        RecipeStatRowData[] stats = new RecipeStatRowData[] {
            RecipeStatRowData.ToRecipeRowData("Health",recipe.outcomeMonster.health),
            RecipeStatRowData.ToRecipeRowData("Attack",recipe.outcomeMonster.attack),
            RecipeStatRowData.ToRecipeRowData("Defence",recipe.outcomeMonster.defense),
            RecipeStatRowData.ToRecipeRowData("Immunity",recipe.outcomeMonster.immunity),
        };

        i = 0;
        foreach (var item in stats)
        {
            if(i < m_StatRowParent.childCount)
            {
                if(m_StatRowParent.GetChild(i).gameObject.TryGetComponent(out RecipeStatRow statRow))
                {
                    statRow.SetData(item);
                }
            }
            else
            {
                RecipeStatRow statRow = Instantiate(m_StatRowPrefab, m_StatRowParent);
                statRow.SetData(item);
            }

            i++;
        }
    }
}
