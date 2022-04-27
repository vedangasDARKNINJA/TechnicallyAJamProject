using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)),RequireComponent(typeof(Collider))]
public class PickUpObject : MonoBehaviour,IPickableObject,ISelectableObject
{
    private Rigidbody m_RigidBody = null;

    protected Rigidbody pickupRigidbody => m_RigidBody;

    private Collider m_Collider = null;
    
    protected Collider pickupCollider=>m_Collider;

    private Material m_Material = null;

    private int m_IsSelectedID = 0;

    protected Material pickupMaterial =>m_Material;

    [SerializeField]
    protected float forceBoost = 2.0f;

    private void Awake()
    {
        m_Material = GetComponent<MeshRenderer>().material;
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        m_IsSelectedID = Shader.PropertyToID("_IsSelected");
    }

    public virtual void OnDrop(Vector3 forward)
    {
        SetRigidbodyEnabled(true);
        Vector3 forwardNorm = forward.normalized;
        transform.rotation = Quaternion.LookRotation(forwardNorm);
        m_RigidBody.AddForce(forceBoost * forwardNorm,ForceMode.Impulse);
        m_RigidBody.AddForce(forceBoost * Vector3.down,ForceMode.Impulse);
    }

    public virtual void OnPickUp()
    {
        m_Material.SetInt(m_IsSelectedID, 0);
        SetRigidbodyEnabled(false);
    }

    public virtual void OnSelected()
    {
        m_Material.SetInt(m_IsSelectedID, 1);
    }

    public virtual void OnDeSelected()
    {
        m_Material.SetInt(m_IsSelectedID, 0);
    }

    protected void SetRigidbodyEnabled(bool enabled)
    {
        m_RigidBody.isKinematic = !enabled;
        m_Collider.enabled = enabled;
    }
}
