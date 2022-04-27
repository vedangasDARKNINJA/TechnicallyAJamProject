using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickableItemDetector : MonoBehaviour
{
    public float radius = 3;

    [SerializeField]
    private DetectorWeightFunctionSO m_WeightFunction;

    [SerializeField]
    private LayerMask m_PickablesMask;

    private float m_BestWeightedScore = float.MaxValue;

    private GameObject m_SelectedGameObject = null;

    [SerializeField]
    private FollowSlot m_FollowSlot = null;

    private bool m_HasPickedUpItem = false;
    public bool hasPickedUpItem => m_HasPickedUpItem;


    private bool IsADropTarget => m_SelectedGameObject != null && m_SelectedGameObject.GetComponent<IDropTargetObject>() != null;
    private bool IsAPickableObject => m_SelectedGameObject != null && m_SelectedGameObject.GetComponent<IPickableObject>() != null;

    private void OnEnable()
    {
        GameInputActions inputActions = PlayerInputController.current?.GetInputActions();
        if (inputActions != null)
        {
            inputActions.Player.Pickup.performed += OnPickUpActionReceived;
        }
    }

    private void OnDisable()
    {
        GameInputActions inputActions = PlayerInputController.current?.GetInputActions();
        if (inputActions != null)
        {
            inputActions.Player.Pickup.performed -= OnPickUpActionReceived;
        }
    }

    public void FixedUpdate()
    {
        Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, radius, m_PickablesMask);
        if (m_SelectedGameObject != null)
        {
            m_SelectedGameObject.GetComponent<ISelectableObject>()?.OnDeSelected();
            m_SelectedGameObject = null;
        }

        m_BestWeightedScore = float.MaxValue;

        foreach (var collider in overlappingColliders)
        {
            Vector3 direction = collider.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, direction);
            float distanceSq = direction.sqrMagnitude;
            float weight = m_WeightFunction.CalculateWeight(distanceSq, angle);

            if (weight < m_BestWeightedScore)
            {
                m_BestWeightedScore = weight;
                m_SelectedGameObject = collider.gameObject;
            }
        }

        if (m_SelectedGameObject != null)
        {
            // Do the pickup logic
            if ((!m_HasPickedUpItem && IsAPickableObject) || (m_HasPickedUpItem && IsADropTarget))
            {
                m_SelectedGameObject.GetComponent<ISelectableObject>()?.OnSelected();
            }

#if UNITY_EDITOR
            Debug.DrawLine(transform.position, m_SelectedGameObject.transform.position, Color.green);
#endif
        }
    }

    private void OnPickUpActionReceived(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!m_HasPickedUpItem)
            {
                m_HasPickedUpItem = true;
                if(IsAPickableObject)
                {
                    m_SelectedGameObject.GetComponent<IPickableObject>()?.OnPickUp();
                    m_FollowSlot.FillSlot(m_SelectedGameObject);
                }
            }
            else if (m_HasPickedUpItem)
            {
                m_HasPickedUpItem = false;
                GameObject target = m_FollowSlot.Target;
                if(m_SelectedGameObject != null)
                {
                    m_FollowSlot.Empty();
                    m_SelectedGameObject?.GetComponent<IDropTargetObject>()?.OnObjectDropped(target);
                }
                else
                {
                    m_FollowSlot.Empty();
                    target?.GetComponent<IPickableObject>()?.OnDrop(transform.forward);
                }
                m_SelectedGameObject = null;
            }
        }
    }

}
