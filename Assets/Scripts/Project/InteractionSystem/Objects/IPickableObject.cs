using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickableObject
{
    ObjectTypes GetObjectTypes();

    void OnPickUp();

    void OnDrop(Vector3 forward);
}
