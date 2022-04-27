using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickableItemDetector : MonoBehaviour
{
    public float radius = 3;
    //public float angleRange = 25;

    [SerializeField]
    private DetectorWeightFunctionSO m_WeightFunction;

    [SerializeField]
    private LayerMask m_PickablesMask;

    private float m_CurrentBestWeightedScore = float.MaxValue;
    private float m_CurrentBestAngle = float.MaxValue;
    private IPickableObject m_CurrentSelectedObject = null;
#if UNITY_EDITOR
    private Collider m_CurrentSelectedCollider = null;
#endif

    public bool hasPickedUpItem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        Collider[] overlappingColliders = Physics.OverlapSphere(transform.position, radius, m_PickablesMask);
        if(m_CurrentSelectedObject!=null)
        {
            m_CurrentSelectedObject.OnDeSelected();
            m_CurrentSelectedObject = null;
        }
        m_CurrentBestWeightedScore = float.MaxValue;
        m_CurrentBestAngle = float.MaxValue;
        foreach (var collider in overlappingColliders)
        {
            IPickableObject pickable = collider.gameObject.GetComponent<IPickableObject>();
            if (pickable != null)
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, direction);
                float distanceSq = direction.sqrMagnitude;
                float weight = m_WeightFunction.CalculateWeight(distanceSq, angle);
                if (weight < m_CurrentBestWeightedScore)
                {
                    m_CurrentBestWeightedScore = weight;
                    m_CurrentBestAngle = angle;
                    m_CurrentSelectedObject = pickable;
#if UNITY_EDITOR
                    m_CurrentSelectedCollider = collider;
#endif
                }
#if UNITY_EDITOR
                Debug.DrawLine(transform.position, collider.transform.position, Color.red);
#endif
            }
        }

        if (m_CurrentSelectedObject != null)
        {
            // Do the pickup logic
            m_CurrentSelectedObject.OnSelected();
#if UNITY_EDITOR
            Debug.DrawLine(transform.position, m_CurrentSelectedCollider.transform.position, Color.green);
#endif
        }
    }
}
