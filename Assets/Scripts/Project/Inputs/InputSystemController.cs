using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystemController : MonoBehaviour
{
    public static InputSystemController instance = null;

    private GameInputActions m_GameInputActions = null;

    public GameInputActions gameInputActions =>m_GameInputActions;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_GameInputActions = new GameInputActions();
        m_GameInputActions.Player.Enable();
    }
}
