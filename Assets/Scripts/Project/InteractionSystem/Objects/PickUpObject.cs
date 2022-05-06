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

    private Material m_Material = null;

    private int m_IsSelectedID = 0;

    protected Material pickupMaterial => m_Material;

    public bool isSelected => m_IsSelected;

    [SerializeField]
    protected float forceBoostForward = 2.0f;

    [SerializeField]
    protected float forceBoostDown = 2.0f;

    public ObjectTypes objectTypes = 0;

    private bool m_IsSelected = false;
    private bool m_IsInteractive = false;

    private InteractionPromptComponent m_InteractionPrompt = null;
    private void Awake()
    {
        m_Material = GetComponent<MeshRenderer>().material;
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_InteractionPrompt = GetComponent<InteractionPromptComponent>();

        m_IsSelectedID = Shader.PropertyToID("_IsSelected");
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
        m_Material.SetInt(m_IsSelectedID, 0);
        SetRigidbodyEnabled(false);
        m_IsInteractive = false;
    }

    public virtual void OnSelected()
    {
        if (!m_IsInteractive)
        {
            return;
        }

        if (!m_IsSelected)
        {
            m_IsSelected = true;
            m_Material.SetInt(m_IsSelectedID, 1);
            m_InteractionPrompt.Show();
        }
    }

    public virtual void OnDeSelected()
    {
        if (!m_IsInteractive)
        {
            return;
        }
        if (m_IsSelected)
        {
            m_IsSelected = false;
            m_Material.SetInt(m_IsSelectedID, 0);
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
            m_IsInteractive = true;
        }
    }
}
