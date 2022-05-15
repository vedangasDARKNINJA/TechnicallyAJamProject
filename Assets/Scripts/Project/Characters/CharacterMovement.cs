using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerSettingsSO m_PlayerSettings = null;

    private Rigidbody m_Rigidbody = null;

    private IInputController m_InputController = null;


    private Vector3 m_RotationVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_InputController = GetComponent<IInputController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velocityNorm = m_InputController.GetInputDirectionNormalized();
        Vector3 flatVelocityNorm = new Vector3(velocityNorm.x, 0, velocityNorm.y);
        Vector2 velocityVector = m_PlayerSettings.moveSpeed * velocityNorm;
                
        m_Rigidbody.velocity = new Vector3(velocityVector.x, m_Rigidbody.velocity.y, velocityVector.y);

        if (!(m_Rigidbody.velocity.magnitude > 0.1f) || flatVelocityNorm == Vector3.zero) return;

        Quaternion newRotation = Quaternion.LookRotation(flatVelocityNorm);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, m_PlayerSettings.rotationSpeed * Time.deltaTime);
    }

}
