using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropTargetObject
{
    void OnObjectDropped(GameObject droppedObject);
}
