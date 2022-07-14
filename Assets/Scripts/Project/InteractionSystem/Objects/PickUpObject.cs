using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)),
    RequireComponent(typeof(Collider)),
    RequireComponent(typeof(InteractionPromptComponent))]
public class PickUpObject : MonoBehaviour, IPickableObject, ISelectableObject
{
    private Rigidbody m_RigidBody = null;

    protected Rigidbody pickupRigidbody => m_RigidBody;

    private Collider m_Collider = null;

    protected Collider pickupCollider => m_Collider;

    private bool m_IsSelected = false;
    private bool m_IsSelectable = false;
    public bool isSelected => m_IsSelected;
    public bool isSelectable => m_IsSelectable;

    [SerializeField]
    protected float forceBoostForward = 2.0f;

    [SerializeField]
    protected float forceBoostDown = 2.0f;

    public ObjectTypes objectTypes = 0;

    

    private InteractionPromptComponent m_InteractionPrompt = null;
    protected virtual void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_InteractionPrompt = GetComponent<InteractionPromptComponent>();

    }

    public virtual void OnDrop(Vector3 forward)
    {
        SetRigidbodyEnabled(true);
        Vector3 forwardNorm = forward.normalized;
        transform.rotation = Quaternion.LookRotation(forwardNorm);
        m_RigidBody.AddForce(forceBoostForward * forwardNorm, ForceMode.Impulse);
        m_RigidBody.AddForce(forceBoostDown * Vector3.down, ForceMode.Impulse);
    }

    public virtual void OnPickUp()
    {
        SetRigidbodyEnabled(false);
        m_IsSelectable = false;
    }

    public virtual void OnSelected()
    {
        if (!m_IsSelectable)
        {
            return;
        }

        if (!m_IsSelected)
        {
            m_IsSelected = true;
            m_InteractionPrompt.Show();
        }
    }

    public virtual void OnDeSelected()
    {
        if (!m_IsSelectable)
        {
            return;
        }
        if (m_IsSelected)
        {
            m_IsSelected = false;
            m_InteractionPrompt.Hide();
        }
    }

    protected void SetRigidbodyEnabled(bool enabled)
    {
        m_RigidBody.isKinematic = !enabled;
        m_Collider.enabled = enabled;
    }

    public ObjectTypes GetObjectTypes()
    {
        return objectTypes;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_RigidBody.isKinematic = true;
            m_IsSelectable = true;
        }
    }
}
