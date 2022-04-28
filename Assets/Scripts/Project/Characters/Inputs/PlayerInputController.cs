using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInputController : MonoBehaviour, IInputController
{
    public Vector2 GetInputDirection()
    {
        return InputSystemController.instance.gameInputActions.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetInputDirectionNormalized()
    {
        return InputSystemController.instance.gameInputActions.Player.Movement.ReadValue<Vector2>().normalized;
    }
}
