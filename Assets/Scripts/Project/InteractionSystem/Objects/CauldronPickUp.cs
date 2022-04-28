using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronPickUp : PickUpObject, IDropTargetObject
{
    public ObjectTypes acceptTypes;

    public bool AcceptsObjectType(ObjectTypes objectType)
    {
        return ((int)acceptTypes & (int)objectType) != 0;
    }

    public void OnObjectDropped(GameObject droppedObject)
    {
        // Todo: check if ingredient matching recipe


        Destroy(droppedObject);
    }
}
