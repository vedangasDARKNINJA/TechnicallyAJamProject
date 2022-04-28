using UnityEngine;

class FollowSlot:MonoBehaviour
{
    private GameObject m_Target = null;

    public GameObject Target => m_Target;

    private bool m_SlotFilled = false;

    public void FillSlot(GameObject target)
    {
        m_Target = target;
        m_SlotFilled = true;
    }

    public void Empty()
    {
        m_Target = null;
        m_SlotFilled = false;
    }

    private void LateUpdate()
    {
        if (!m_SlotFilled)
        {
            return;
        }

        m_Target.transform.position = transform.position;
        m_Target.transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}

