using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class PromptButtonState
{
    public string stateName;
    public string[] buttons;
}

public class InteractionPromptComponent : MonoBehaviour
{
    public static event Action<InteractionPromptComponent> OnAdded;
    public static event Action<InteractionPromptComponent> OnHide;
    public static event Action<InteractionPromptComponent> OnShow;
    public static event Action<InteractionPromptComponent> OnRemoved;
    public static event Action<InteractionPromptComponent> OnPromptDataChanged;

    [SerializeField]
    private PromptButtonState[] m_ButtonStates;

    private Dictionary<string, PromptButtonState> m_Buttons = new Dictionary<string, PromptButtonState>();

    private string m_CurrentState;

    private void Awake()
    {
        foreach (var promptData in m_ButtonStates)
        {
            if (!m_Buttons.ContainsKey(promptData.stateName))
            {
                m_Buttons.Add(promptData.stateName, promptData);
            }
        }
        if (m_ButtonStates.Length > 0)
        {
            m_CurrentState = m_ButtonStates[0].stateName;
        }
    }

    private void Start()
    {
        OnAdded?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnRemoved?.Invoke(this);
    }

    public void Show()
    {
        OnShow?.Invoke(this);
    }

    public void Hide()
    {
        OnHide?.Invoke(this);
    }

    public PromptButtonState GetStateData()
    {
        if (m_Buttons.ContainsKey(m_CurrentState))
        {
            return m_Buttons[m_CurrentState];
        }
        return null;
    }

    public void SetState(string state)
    {
        if (m_CurrentState != state)
        {
            m_CurrentState = state;
            OnPromptDataChanged?.Invoke(this);
        }
    }
}
