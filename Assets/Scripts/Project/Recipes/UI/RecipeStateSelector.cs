using DG.Tweening;
using UnityEngine;

public class RecipeStateSelector : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [SerializeField]
    private Transform m_RecipeProgress;

    [SerializeField]
    private Transform m_MonsterProgress;


    private void OnEnable()
    {
        CauldronManager.OnCauldronStateChanged += OnCauldronStateChanged;
    }

    private void OnDisable()
    {
        CauldronManager.OnCauldronStateChanged -= OnCauldronStateChanged;
    }

    private void OnDestroy()
    {
        CauldronManager.OnCauldronStateChanged -= OnCauldronStateChanged;
    }


    private void Start()
    {
        m_CanvasGroup.alpha = 0.0f;
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
    }
    private void OnCauldronStateChanged(CauldronState prevState, CauldronState newState)
    {
        if (newState == CauldronState.WaitingIngredients)
        {
            if (m_CanvasGroup.alpha != 1.0f)
            {
                CanvasFade(1.0f, 0.25f, true);
            }
            m_MonsterProgress.gameObject.SetActive(false);
            m_RecipeProgress.gameObject.SetActive(true);
            m_RecipeProgress.DOScale(1.0f, 0.25f)
                .From(0.0f)
                .SetEase(Ease.InOutCubic);
        }
        else if (newState == CauldronState.CookingMonster)
        {
            if (m_CanvasGroup.alpha != 1.0f)
            {
                CanvasFade(1.0f, 0.25f, true);
            }
            m_RecipeProgress.gameObject.SetActive(false);
            m_MonsterProgress.gameObject.SetActive(true);
            m_MonsterProgress.DOScale(1.0f, 0.25f)
                .From(0.0f)
                .SetEase(Ease.InOutCubic);
        }
        else if (newState != CauldronState.WaitingIngredients || newState != CauldronState.CookingMonster)
        {
            if (m_CanvasGroup.alpha != 0.0f)
            {
                CanvasFade(0.0f, 0.2f, false);
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    m_MonsterProgress.gameObject.SetActive(false);
                    m_RecipeProgress.gameObject.SetActive(true);
                });
            }
        }
    }

    private void CanvasFade(float alpha, float duration, bool interactable)
    {
        if (interactable)
        {
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
        }
        else
        {
            m_CanvasGroup.interactable = interactable;
            m_CanvasGroup.blocksRaycasts = interactable;
        }

        m_CanvasGroup.DOFade(alpha, duration)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                if (interactable)
                {
                    m_CanvasGroup.interactable = true;
                    m_CanvasGroup.blocksRaycasts = true;
                }
            });
    }
}
