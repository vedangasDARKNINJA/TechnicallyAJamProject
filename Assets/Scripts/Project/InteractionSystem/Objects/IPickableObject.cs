using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickableObject
{
    void OnSelected();

    void OnDeSelected();

    void OnPickUp();

    void OnDrop();
}
