using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InteractionPromptWidget : MonoBehaviour
{
    [SerializeField]
    private RectTransform buttonPrompt = null;

    [SerializeField]
    private TextMeshProUGUI buttonText = null;

    public bool animate = true;

    public void Init(string button)
    {
        buttonText.text = button;
    }

    public void Show(float showDuration)
    {
        if (animate)
        {
            buttonPrompt.DOKill();
            buttonPrompt.DOScale(1.0f, showDuration)
                .From(0.0f)
                .SetEase(Ease.OutExpo);
        }
        else
        {
            buttonPrompt.transform.localScale = Vector3.one;
        }
    }

    public void Hide(float hideDuration)
    {
        if (animate)
        {
            buttonPrompt.DOKill();
            buttonPrompt.DOScale(0.0f, hideDuration)
                .SetEase(Ease.OutQuint);
        }
        else
        {
            buttonPrompt.transform.localScale = Vector3.zero;
        }
    }

}
