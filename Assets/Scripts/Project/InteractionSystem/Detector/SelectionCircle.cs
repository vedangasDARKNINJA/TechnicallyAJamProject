using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionState
{
    None,
    Selected,
    Incompatible,
    Deselected
}

[RequireComponent(typeof(SpriteRenderer))]
public class SelectionCircle : MonoBehaviour
{
    [System.Serializable]
    private struct AnimationData
    {
        public float duration;
        public Ease ease;
        public Color color;
    }

    private SpriteRenderer m_Renderer = null;
    [SerializeField]
    private float m_GroundOffset = 0.2f;

    [SerializeField]
    private bool m_Animate = true;

    [SerializeField]
    private AnimationData m_SelectedData;

    [SerializeField]
    private AnimationData m_IncompatibleData;

    [SerializeField]
    private AnimationData m_DeselectedData;

    private SelectionState m_CurrentState;
    private Transform m_CurrentTransform;

    [SerializeField]
    private float m_Scale = 2.0f;

    // Start is called before the first frame update
    void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        StateNone(true);
    }

    public void SetState(SelectionState state, Transform selectedTransform)
    {
        if (m_CurrentState != state || m_CurrentTransform != selectedTransform)
        {
            if (selectedTransform == null)
            {
                m_CurrentState = state;
                StateNone(true);
                return;
            }
            m_CurrentTransform = selectedTransform;
            switch (state)
            {
                case SelectionState.Selected:
                    {
                        StateSelected();
                        break;
                    }
                case SelectionState.Incompatible:
                    {
                        StateIncompatible();
                        break;
                    }
                case SelectionState.Deselected:
                    {
                        StateDeselected();
                        break;
                    }
                case SelectionState.None:
                    {
                        StateNone(true);
                        break;
                    }
                default:
                    break;
            }
            m_CurrentState = state;
        }
    }


    void StateSelected()
    {
        gameObject.SetActive(true);
        transform.position = m_CurrentTransform.position + m_GroundOffset * Vector3.up;
        transform.DOKill();
        if (m_Animate)
        {
            transform.DOScale(m_Scale, m_SelectedData.duration)
                .From(0.0f)
                .SetEase(m_SelectedData.ease);
        }
        else
        {
            transform.localScale = m_Scale * Vector3.one;
        }

        m_Renderer.DOKill();
        if (m_Animate)
        {
            m_Renderer.DOColor(m_SelectedData.color, m_SelectedData.duration)
                .From(m_Renderer.color)
                .SetEase(Ease.InOutQuad);
        }
        else
        {
            m_Renderer.color = m_SelectedData.color;
        }
    }

    void StateDeselected()
    {
        transform.DOKill();
        transform.position = m_CurrentTransform.position + m_GroundOffset * Vector3.up;
        gameObject.SetActive(false);
    }

    void StateIncompatible()
    {
        gameObject.SetActive(true);
        transform.position = m_CurrentTransform.position + m_GroundOffset * Vector3.up;
        transform.DOKill();
        if (m_Animate)
        {
            transform.DOScale(m_Scale, m_SelectedData.duration)
                .From(0.0f)
                .SetEase(m_IncompatibleData.ease);
        }
        else
        {
            transform.localScale = m_Scale * Vector3.one;
        }

        m_Renderer.DOKill();
        if (m_Animate)
        {
            m_Renderer.DOColor(m_IncompatibleData.color, m_IncompatibleData.duration)
                .From(m_Renderer.color)
                .SetEase(Ease.InOutQuad);
        }
        else
        {
            m_Renderer.color = m_IncompatibleData.color;
        }
    }

    void StateNone(bool skipAnimation = false)
    {
        transform.DOKill();

        if (m_CurrentTransform)
        {
            transform.position = m_CurrentTransform.position + m_GroundOffset * Vector3.up;
        }

        if (m_Animate && !skipAnimation)
        {
            transform.DOScale(0.0f, 0.2f)
                .SetEase(Ease.Linear);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
