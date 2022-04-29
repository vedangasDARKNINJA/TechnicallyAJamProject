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
        // Todo: check if ingredient matching recipe
        if(m_Manager!= null)
        {
            m_Manager.AddIngredient(droppedObject);
        }
        Destroy(droppedObject);
    }
}
