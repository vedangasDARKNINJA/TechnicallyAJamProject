using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class RecipePanelUI : MonoBehaviour
{
    [SerializeField]
    private Button m_CancelButton;
        
    private CanvasGroup m_CanvasGroup = null;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }


    private void OnEnable()
    {
        m_CancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnDisable()
    {
        m_CancelButton.onClick.RemoveListener(OnCancelButtonClicked);
    }

    void OnCancelButtonClicked()
    {
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.DOKill();
        m_CanvasGroup.DOFade(0.0f, 0.3f)
            .SetEase(Ease.InOutQuad);
    }

    void OnShowPanel()
    {
        m_CanvasGroup.DOKill();
        m_CanvasGroup.DOFade(0.0f, 0.3f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(()=>m_CanvasGroup.blocksRaycasts=true);
    }
}
