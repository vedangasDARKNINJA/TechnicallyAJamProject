using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PickableItemDetector))]
public class PickableItemDetectorEditor : Editor
{
    private PickableItemDetector m_PickableItemDetector = null;
    private void OnEnable()
    {
        m_PickableItemDetector = (PickableItemDetector)target;
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(m_PickableItemDetector.transform.position, Vector3.up, m_PickableItemDetector.radius);
        /*
        Vector3 direction = Quaternion.AngleAxis(-0.5f * Mathf.Abs(m_PickableItemDetector.angleRange),Vector3.up) * m_PickableItemDetector.transform.forward;
        Handles.DrawSolidArc(m_PickableItemDetector.transform.position, Vector3.up, direction, Mathf.Abs(m_PickableItemDetector.angleRange), m_PickableItemDetector.radius);
        */
    }
}
