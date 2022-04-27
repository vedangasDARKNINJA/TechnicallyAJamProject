using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronPickUp : PickUpObject, IDropTargetObject
{
    public void OnObjectDropped(GameObject droppedObject)
    {
        // Todo: check if ingredient matching recipe


        Destroy(droppedObject);
    }
}
