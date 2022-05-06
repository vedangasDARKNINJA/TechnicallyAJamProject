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

    public void Init(string button)
    {
        buttonText.text = button;
    }

    public void Show(float showDuration)
    {
        buttonPrompt.DOKill();
        buttonPrompt.transform.localScale = Vector3.zero;
        buttonPrompt.DOScale(1.0f, showDuration)
            .SetEase(Ease.OutExpo);
    }

    public void Hide(float hideDuration)
    {
        buttonPrompt.DOKill();
        buttonPrompt.DOScale(0.0f, hideDuration)
            .SetEase(Ease.OutQuint);
    }

}
