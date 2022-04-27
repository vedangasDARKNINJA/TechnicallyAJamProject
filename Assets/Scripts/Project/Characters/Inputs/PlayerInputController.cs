using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInputController : MonoBehaviour,IInputController
{
    GameInputActions m_GameInputActions = null;
    Camera m_MainCamera = null;

    public Vector3 GetFacingDirection()
    {
        if (m_MainCamera != null)
        {
            Ray mouseRay = m_MainCamera.ScreenPointToRay(m_GameInputActions.Player.Aim.ReadValue<Vector2>());
            if (Physics.Raycast(mouseRay,out RaycastHit raycastHit))
            {
                Vector3 direction = raycastHit.point - transform.position;
                direction.y = 0;
                return direction;
            }
        }
        return Vector3.forward;
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
        m_GameInputActions = new GameInputActions();
        m_GameInputActions.Player.Enable();

        m_MainCamera = Camera.main; 
    }
}
