using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CauldronState
{
    None,
    Disabled,
    Available,
    Picked,
    WaitingIngredients,
    CookingMonster,
    CancellingRecipe
}

public class CauldronManager : MonoBehaviour
{
    public static event Action OnChooseRecipe;
    public static event Action<CauldronState, CauldronState> OnCauldronStateChanged;
    public static event Action<float> OnMonsterProgress;
    public static event Action<Dictionary<int, int>> OnRecipeIngredientAdded;

    private InteractionPromptComponent m_InteractionPrompt = null;

    [SerializeField]
    private RecipeSO m_CurrentRecipe = null;

    [SerializeField]
    private float m_CookTime = 4.0f;

    private Dictionary<int, int> m_RecipeStats = new Dictionary<int, int>();

    private CauldronState m_CurrentState = CauldronState.None;
    private CauldronState m_CachedState = CauldronState.None;
    private bool m_ActionPaused = false;

    // Stores all the submitted objects and thorws out when the recipe is cancelled
    [SerializeField]
    private List<GameObject> m_CauldronItemInventory = new List<GameObject>();
    private WaitForSeconds m_WaitForSeconds = new WaitForSeconds(0.2f);

    private IEnumerator m_CurrentCoroutine = null;

    public void RestorePreviousState()
    {
        SetState(m_CachedState);
        m_ActionPaused = false;
    }

    private void Awake()
    {
        m_InteractionPrompt = GetComponent<InteractionPromptComponent>();
        GameInputActions inputActions = InputSystemController.instance.gameInputActions;
        inputActions.Player.Recipes.performed += OnRecipesInputRecieved;
    }

    private void OnEnable()
    {
        RecipeChoicePanel.OnRecipeSelected += OnRecipeSelected;
        RecipeStatsUI.OnRecipeCancelled += OnRecipeCancelled;
    }

    private void OnDisable()
    {
        RecipeChoicePanel.OnRecipeSelected -= OnRecipeSelected;
        RecipeStatsUI.OnRecipeCancelled -= OnRecipeCancelled;
    }

    private void OnDestroy()
    {
        RecipeChoicePanel.OnRecipeSelected -= OnRecipeSelected;
        RecipeStatsUI.OnRecipeCancelled -= OnRecipeCancelled;
    }

    private void Start()
    {
        SetState(CauldronState.Available);
    }

    private void OnRecipesInputRecieved(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (m_CurrentState != CauldronState.Available)
        {
            return;
        }

        OnChooseRecipe?.Invoke();
        m_CurrentState = CauldronState.Disabled;
    }

    public void OnRecipeSelected(RecipeSO recipe)
    {
        if (recipe == null)
        {
            SetState(CauldronState.Available);
            return;
        }
        else
        {
            m_CurrentRecipe = recipe;
            m_InteractionPrompt.SetState("Busy");
            SetState(CauldronState.WaitingIngredients);
        }

        m_RecipeStats.Clear();
        foreach (var data in recipe.RecipeData)
        {
            m_RecipeStats.Add(data.ingredient.identifier.id, (int)data.quantity);
        }
    }

    private void OnRecipeCancelled()
    {
        m_CurrentRecipe = null;
        m_RecipeStats.Clear();
        StartAction(EmptyCauldronCoroutine());
    }

    public void AddIngredient(GameObject ingredientObject)
    {
        if (m_CurrentRecipe == null || m_CurrentState != CauldronState.WaitingIngredients)
        {
            if (ingredientObject.gameObject.TryGetComponent(out PickUpObject pickUp))
            {
                Vector3 direction = UnityEngine.Random.onUnitSphere;
                direction.y = 0;
                ingredientObject.transform.position = transform.position + 1.5f * Vector3.up;
                pickUp.OnDrop(direction);
            }
            return;
        }

        ingredientObject.gameObject.SetActive(false);
        m_CauldronItemInventory.Add(ingredientObject);

        IngredientObject ingredientComponent = ingredientObject.GetComponent<IngredientObject>();
        if (ingredientObject)
        {
            int id = ingredientComponent.identifier.id;

            if (m_RecipeStats.ContainsKey(id))
            {
                m_RecipeStats[id]--;
                m_RecipeStats[id] = Mathf.Max(m_RecipeStats[id], 0);
                if (m_RecipeStats[id] == 0)
                {
                    m_RecipeStats.Remove(id);
                }
                OnRecipeIngredientAdded?.Invoke(m_RecipeStats);
            }
        }

        if (m_RecipeStats.Count == 0)
        {
            StartAction(CookMonsterCoroutine());
        }
    }

    public void SetState(CauldronState state)
    {
        if (m_CurrentState != state)
        {
            OnCauldronStateChanged?.Invoke(m_CurrentState, state);

            if (state == CauldronState.Picked)
            {
                m_ActionPaused = true;
                m_CachedState = m_CurrentState;
            }

            m_CurrentState = state;
        }
    }

    private void StartAction(IEnumerator enumerator)
    {
        if (m_CurrentCoroutine != null)
        {
            StopCoroutine(m_CurrentCoroutine);
        }
        m_CurrentCoroutine = enumerator;
        StartCoroutine(enumerator);
    }

    IEnumerator EmptyCauldronCoroutine()
    {
        SetState(CauldronState.CancellingRecipe);
        for (int i = m_CauldronItemInventory.Count - 1; i >= 0; i--)
        {
            while (m_ActionPaused)
            {
                yield return null;
            }
            m_CauldronItemInventory[i].gameObject.SetActive(true);
            m_CauldronItemInventory[i].transform.position = transform.position + 1.5f * Vector3.up;
            if (m_CauldronItemInventory[i].gameObject.TryGetComponent(out PickUpObject pickUp))
            {
                Vector3 direction = UnityEngine.Random.onUnitSphere;
                direction.y = 0;
                pickUp.OnDrop(direction);
            }
            m_CauldronItemInventory.RemoveAt(i);
            yield return m_WaitForSeconds;
        }
        m_InteractionPrompt.SetState("Vacant");
        SetState(CauldronState.Available);
    }

    IEnumerator CookMonsterCoroutine()
    {
        SetState(CauldronState.CookingMonster);
        foreach (var item in m_CauldronItemInventory)
        {
            Destroy(item);
        }
        m_CauldronItemInventory.Clear();
        float cookTime = m_CookTime;
        while (cookTime > 0)
        {
            if (m_ActionPaused)
            {
                yield return null;
            }
            cookTime -= Time.deltaTime;
            cookTime = Mathf.Max(cookTime, 0.0f);
            OnMonsterProgress?.Invoke(1.0f - (cookTime / m_CookTime));
            yield return null;
        }

        // TODO: Spawn the actual monster! 
        if (m_CurrentRecipe)
        {
            GameObject monster = Instantiate(m_CurrentRecipe.outcomeMonster.prefab);
            monster.transform.position = transform.position + 3* Vector3.up;
            if(monster.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddForce(10f * UnityEngine.Random.onUnitSphere);
            }
        }

        m_InteractionPrompt.SetState("Vacant");
        SetState(CauldronState.Available);
    }
}
