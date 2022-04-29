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
    private AnimationData m_SelectedData;

    [SerializeField]
    private AnimationData m_IncompatibleData;

    [SerializeField]
    private AnimationData m_DeselectedData;

    private SelectionState m_CurrentState;
    private Transform m_CurrentTransform;
    private Vector3 m_InitialScale = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_InitialScale = transform.localScale;

        StateNone();
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
                        StateNone();
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
        transform.position = m_CurrentTransform.position + m_GroundOffset * Vector3.up;
        transform.DOScale(m_InitialScale, m_SelectedData.duration)
            .From(Vector3.zero)
            .SetEase(m_SelectedData.ease);

        m_Renderer.DOColor(m_SelectedData.color, m_SelectedData.duration)
            .From(m_Renderer.color)
            .SetEase(Ease.InOutQuad);
    }

    void StateDeselected()
    {
        transform.position = m_CurrentTransform.position + m_GroundOffset * Vector3.up;
        transform.DOScale(Vector3.zero, m_DeselectedData.duration)
            .SetEase(m_DeselectedData.ease);
    }

    void StateIncompatible()
    {
        transform.position = m_CurrentTransform.position + m_GroundOffset * Vector3.up;
        transform.DOScale(m_InitialScale, m_SelectedData.duration)
            .From(Vector3.zero)
            .SetEase(m_IncompatibleData.ease);

        m_Renderer.DOColor(m_IncompatibleData.color, m_IncompatibleData.duration)
            .From(m_Renderer.color)
            .SetEase(Ease.InOutQuad);
    }

    void StateNone(bool skipAnimation = false)
    {
        if (skipAnimation)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            if (m_CurrentTransform)
            {
                transform.position = m_CurrentTransform.position + m_GroundOffset * Vector3.up;
            }

            transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.Linear);
        }
    }
}
