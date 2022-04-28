using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropTargetObject
{
    bool AcceptsObjectType(ObjectTypes objectType);

    void OnObjectDropped(GameObject droppedObject);

}
