using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickableItemDetector : MonoBehaviour
{
    private static T AsA<T>(GameObject obj)
    {
        T component = default;
        if (obj != null)
        {
            component = obj.GetComponent<T>();
        }
        return component;
    }

    public float radius = 3;

    [SerializeField]
    private DetectorWeightFunctionSO m_WeightFunction;

    [SerializeField]
    private LayerMask m_PickablesMask;

    private float m_BestWeightedScore = float.MaxValue;

    private GameObject m_SelectedGameObject = null;

    private GameObject m_PickedGameObject = null;

    [SerializeField]
    private FollowSlot m_FollowSlot = null;

    private bool m_HasPickedUpItem = false;
    public bool hasPickedUpItem => m_HasPickedUpItem;

    private void Awake()
    {
        print("pick up detector awake");
        GameInputActions inputActions = InputSystemController.instance.gameInputActions;
        inputActions.Player.Pickup.performed += OnPickUpActionReceived;
    }

    public void FixedUpdate()
    {
        Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, radius, m_PickablesMask);
        

        m_BestWeightedScore = float.MaxValue;
        GameObject selectionObject = null;

        foreach (var collider in overlappingColliders)
        {
            Vector3 direction = collider.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, direction);
            float distanceSq = direction.sqrMagnitude;
            float weight = m_WeightFunction.CalculateWeight(distanceSq, angle);

            if (weight < m_BestWeightedScore)
            {
                m_BestWeightedScore = weight;
                selectionObject = collider.gameObject;
            }
        }

        if (selectionObject != null)
        {
            m_SelectedGameObject = selectionObject;
#if UNITY_EDITOR
            Debug.DrawLine(transform.position, selectionObject.transform.position, Color.green);
#endif
            if (m_HasPickedUpItem)
            {
                IDropTargetObject dropTarget = AsA<IDropTargetObject>(m_SelectedGameObject);
                if (dropTarget != null)
                {
                    IPickableObject pickable = AsA<IPickableObject>(m_PickedGameObject);
                    if (pickable != null)
                    {
                        if (dropTarget.AcceptsObjectType(pickable.GetObjectTypes()))
                        {
                            ISelectableObject selectable = AsA<ISelectableObject>(m_SelectedGameObject);
                            if (selectable != null)
                            {
                                selectable.OnSelected();
                            }
                        }
                    }
                }
            }
            else
            {
                ISelectableObject selectable = AsA<ISelectableObject>(m_SelectedGameObject);
                if (selectable != null)
                {
                    selectable.OnSelected();
                }
            }
        }
        else
        {
            if(m_SelectedGameObject!= null)
            {
                ISelectableObject selectable = AsA<ISelectableObject>(m_SelectedGameObject);
                if (selectable != null)
                {
                    selectable.OnDeSelected();
                }
                m_SelectedGameObject = null;
            }
        }
    }

    private void OnPickUpActionReceived(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!m_HasPickedUpItem)
            {
                if (m_SelectedGameObject != null)
                {
                    ISelectableObject selectable = AsA<ISelectableObject>(m_SelectedGameObject);
                    if (selectable != null)
                    {
                        selectable.OnDeSelected();
                    }
                    IPickableObject pickable = AsA<IPickableObject>(m_SelectedGameObject);
                    if (pickable != null)
                    {
                        pickable.OnPickUp();
                        m_PickedGameObject = m_SelectedGameObject;
                        m_FollowSlot.FillSlot(m_PickedGameObject);
                    }
                    m_HasPickedUpItem = true;
                }
            }
            else if (m_HasPickedUpItem)
            {
                IPickableObject pickable = AsA<IPickableObject>(m_PickedGameObject);
                bool dropped = false;
                if (m_SelectedGameObject != null)
                {
                    ISelectableObject selectable = AsA<ISelectableObject>(m_SelectedGameObject);
                    if (selectable != null)
                    {
                        if (selectable.isSelected)
                        {
                            IDropTargetObject dropTarget = AsA<IDropTargetObject>(m_SelectedGameObject);
                            if (dropTarget != null)
                            {
                                dropTarget.OnObjectDropped(m_PickedGameObject);
                            }
                            dropped = true;
                        }
                    }
                }

                m_FollowSlot.Empty();
                m_PickedGameObject = null;
                if (!dropped)
                {
                    if (pickable != null)
                    {
                        pickable.OnDrop(transform.forward);
                    }
                }

                m_HasPickedUpItem = false;
            }
        }

    }
}
