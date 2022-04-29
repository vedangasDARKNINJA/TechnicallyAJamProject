using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class WorldWidgetLayer : MonoBehaviour
{
    public GameObject interactionPromptPrefab;
    public GameObject interactionPromptContainerPrefab;
    private Camera m_MainCamera;
    private class InteractionPromptData
    {
        public bool isShown;
        public GameObject interactionPromptContainer;
        public CanvasGroup containerCanvasGroup;
        public InteractionPromptWidget[] interactionPrompts;
    }

    private Dictionary<InteractionPromptComponent, InteractionPromptData> m_InteractionPrompts = new Dictionary<InteractionPromptComponent, InteractionPromptData>();

    public float showDuration = 0.5f;
    public float hideDuration = 0.4f;

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    private void OnEnable()
    {
        InteractionPromptComponent.OnAdded += InteractionPrompt_OnAdded;
        InteractionPromptComponent.OnShow += InteractionPrompt_OnShow;
        InteractionPromptComponent.OnHide += InteractionPrompt_OnHide;
        InteractionPromptComponent.OnRemoved += InteractionPrompt_OnRemoved;
        InteractionPromptComponent.OnPromptDataChanged += InteractionPromptComponent_OnPromptDataChanged;
    }

    

    private void OnDisable()
    {
        InteractionPromptComponent.OnAdded -= InteractionPrompt_OnAdded;
        InteractionPromptComponent.OnShow -= InteractionPrompt_OnShow;
        InteractionPromptComponent.OnHide -= InteractionPrompt_OnHide;
        InteractionPromptComponent.OnRemoved -= InteractionPrompt_OnRemoved;
        InteractionPromptComponent.OnPromptDataChanged -= InteractionPromptComponent_OnPromptDataChanged;
    }

    private void OnDestroy()
    {
        InteractionPromptComponent.OnAdded -= InteractionPrompt_OnAdded;
        InteractionPromptComponent.OnShow -= InteractionPrompt_OnShow;
        InteractionPromptComponent.OnHide -= InteractionPrompt_OnHide;
        InteractionPromptComponent.OnRemoved -= InteractionPrompt_OnRemoved;
        InteractionPromptComponent.OnPromptDataChanged -= InteractionPromptComponent_OnPromptDataChanged;
    }

    private void InteractionPrompt_OnAdded(InteractionPromptComponent obj)
    {
        InteractionPromptData data = new InteractionPromptData();
        data.isShown = false;
        data.interactionPromptContainer = Instantiate(interactionPromptContainerPrefab, transform);
        data.containerCanvasGroup = data.interactionPromptContainer.GetComponent<CanvasGroup>();
        data.containerCanvasGroup.alpha = 0.0f;

        PromptButtonState buttonState = obj.GetStateData();
        if (buttonState != null)
        {
            data.interactionPrompts = new InteractionPromptWidget[buttonState.buttons.Length];
            for (int i = 0; i < data.interactionPrompts.Length; ++i)
            {
                GameObject go = Instantiate(interactionPromptPrefab, data.interactionPromptContainer.transform);
                data.interactionPrompts[i] = go.GetComponent<InteractionPromptWidget>();
                data.interactionPrompts[i].Init(buttonState.buttons[i]);
            }
            m_InteractionPrompts.Add(obj, data);
        }
    }

    private void InteractionPromptComponent_OnPromptDataChanged(InteractionPromptComponent obj)
    {
        InteractionPromptData data = m_InteractionPrompts[obj];
        foreach(var prompt in data.interactionPrompts)
        {
            Destroy(prompt.gameObject);
        }

        PromptButtonState buttonState = obj.GetStateData();
        if (buttonState != null)
        {
            data.interactionPrompts = new InteractionPromptWidget[buttonState.buttons.Length];
            for (int i = 0; i < data.interactionPrompts.Length; ++i)
            {
                GameObject go = Instantiate(interactionPromptPrefab, data.interactionPromptContainer.transform);
                data.interactionPrompts[i] = go.GetComponent<InteractionPromptWidget>();
                data.interactionPrompts[i].Init(buttonState.buttons[i]);
            }
        }
    }

    private void InteractionPrompt_OnShow(InteractionPromptComponent obj)
    {
        InteractionPromptData data = m_InteractionPrompts[obj];
        data.isShown = true;
        data.containerCanvasGroup.alpha = 1.0f;
        data.interactionPromptContainer.transform.position = m_MainCamera.WorldToScreenPoint(obj.transform.position);
        foreach (var prompt in data.interactionPrompts)
        {
            prompt.Show(showDuration);
        }
    }
    private void InteractionPrompt_OnHide(InteractionPromptComponent obj)
    {
        InteractionPromptData data = m_InteractionPrompts[obj];

        foreach (var prompt in data.interactionPrompts)
        {
            prompt.Hide(hideDuration);
        }
        DOVirtual.DelayedCall(hideDuration, () =>
        {
            data.containerCanvasGroup.alpha = 0.0f;
            data.isShown = false;
        });
    }

    private void InteractionPrompt_OnRemoved(InteractionPromptComponent obj)
    {
        InteractionPromptData data = m_InteractionPrompts[obj];
        Destroy(data.interactionPromptContainer);
        m_InteractionPrompts.Remove(obj);
    }

    private void LateUpdate()
    {
        foreach (KeyValuePair<InteractionPromptComponent, InteractionPromptData> prompts in m_InteractionPrompts)
        {
            if (prompts.Value.isShown)
            {
                prompts.Value.interactionPromptContainer.transform.position = m_MainCamera.WorldToScreenPoint(prompts.Key.transform.position);
            }
        }
    }
}
