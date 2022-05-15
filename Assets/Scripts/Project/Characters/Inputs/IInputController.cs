using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputController 
{
    Vector2 GetInputDirection();
    Vector2 GetInputDirectionNormalized();
    Vector3 GetFacingDirection();
}
