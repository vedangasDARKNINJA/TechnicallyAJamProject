using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInputController : MonoBehaviour,IInputController
{
    public static PlayerInputController current = null;

    GameInputActions m_GameInputActions = null;
    Camera m_MainCamera = null;

    public GameInputActions GetInputActions()
    {
        return m_GameInputActions;
    }

    public Vector2 GetInputDirection()
    {
        return m_GameInputActions.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetInputDirectionNormalized()
    {
        return m_GameInputActions.Player.Movement.ReadValue<Vector2>().normalized;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(current!= this)
        {
            current = this;
        }
        m_GameInputActions = new GameInputActions();
        m_GameInputActions.Player.Enable();

        m_MainCamera = Camera.main; 
    }
}
