using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectableObject
{
    bool isSelected { get; }

    void OnSelected();

    void OnDeSelected();

}
