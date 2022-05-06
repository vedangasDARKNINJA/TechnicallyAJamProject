using System;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct InteractionPromptData
{
    public string button; // could be done better
    
}


public class InteractionPromptComponent : MonoBehaviour
{
    public static event Action<InteractionPromptComponent> OnAdded;
    public static event Action<InteractionPromptComponent> OnHide;
    public static event Action<InteractionPromptComponent> OnShow;
    public static event Action<InteractionPromptComponent> OnRemoved;

    public string[] buttons;

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
}
