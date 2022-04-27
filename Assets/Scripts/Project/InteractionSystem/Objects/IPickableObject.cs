using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickableObject
{
    void OnPickUp();

    void OnDrop(Vector3 forward);
}
