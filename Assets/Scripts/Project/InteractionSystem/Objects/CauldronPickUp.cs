using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronPickUp : PickUpObject, IDropTargetObject
{
    public ObjectTypes acceptTypes;

    private CauldronManager m_Manager = null;

    protected override void Awake()
    {
        base.Awake();
        m_Manager = GetComponent<CauldronManager>();
    }

    public bool AcceptsObjectType(ObjectTypes objectType)
    {
        return ((int)acceptTypes & (int)objectType) != 0;
    }

    public void OnObjectDropped(GameObject droppedObject)
    {
        if(m_Manager!= null)
        {
            m_Manager.AddIngredient(droppedObject);
        }
    }

    public override void OnPickUp()
    {
        base.OnPickUp();
        if(TryGetComponent(out CauldronManager manager))
        {
            manager.SetState(CauldronState.Picked);
        }
    }

    public override void OnDrop(Vector3 forward)
    {
        base.OnDrop(forward);
        if (TryGetComponent(out CauldronManager manager))
        {
            manager.RestorePreviousState();
        }
    }
}
