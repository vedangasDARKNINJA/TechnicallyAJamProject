using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WorldWidgetLayer : MonoBehaviour
{
    public InteractionPromptWidget interactionPromptPrefab;
    public GameObject interactionPromptContainerPrefab;
    private Camera m_MainCamera;

    private class InteractionPromptData
    {
        public bool isShown;
        public GameObject interactionPromptContainer;
        public CanvasGroup containerCanvasGroup;
        public List<InteractionPromptWidget> interactionPrompts;

        public int availableWidgets
        {
            get
            {
                if (interactionPrompts != null)
                {
                    return interactionPrompts.Count;
                }
                return 0;
            }
        }

        public InteractionPromptWidget Get(int index)
        {
            if (index < availableWidgets)
            {
                Transform t = interactionPromptContainer.transform;
                var child = t.GetChild(index);
                child.gameObject.SetActive(true);
                return child.gameObject.GetComponent<InteractionPromptWidget>();
            }

            return null;
        }

        public void DeactivateChildren()
        {
            Transform t = interactionPromptContainer.transform;
            for (int i = 0, count = t.childCount; i < count; ++i)
            {
                var child = t.GetChild(i);
                child.gameObject.SetActive(false);
            }
        }
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
            data.interactionPrompts = new List<InteractionPromptWidget>();
            for (int i = 0; i < buttonState.buttons.Length; ++i)
            {
                var prompt = Instantiate(interactionPromptPrefab, data.interactionPromptContainer.transform);
                prompt.animate = true;
                prompt.Init(buttonState.buttons[i]);
                data.interactionPrompts.Add(prompt);
            }

            m_InteractionPrompts.Add(obj, data);
        }
    }

    private void InteractionPromptComponent_OnPromptDataChanged(InteractionPromptComponent obj)
    {
        InteractionPromptData data = m_InteractionPrompts[obj];

        data.interactionPrompts.Clear();
        data.DeactivateChildren();

        PromptButtonState buttonState = obj.GetStateData();
        if (buttonState != null)
        {
            for (int i = 0; i < buttonState.buttons.Length; ++i)
            {
                InteractionPromptWidget prompt = null;
                if (i < data.availableWidgets)
                {
                    prompt = data.Get(i);
                }
                else
                {
                    prompt = Instantiate(interactionPromptPrefab, data.interactionPromptContainer.transform);
                }
                prompt.Init(buttonState.buttons[i]);
                data.interactionPrompts.Add(prompt);
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
        data.interactionPrompts.Clear();
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
