using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronPickUp : MonoBehaviour, IPickableObject
{
    /*
     * QUICK AND DIRTY
     */
    
    private Material m_Material = null;

    private void Awake()
    {
        m_Material = GetComponent<MeshRenderer>().material;
    }

    public void OnDrop()
    {
        throw new System.NotImplementedException();
    }

    public void OnPickUp()
    {
        throw new System.NotImplementedException();
    }

    public void OnSelected()
    {
        m_Material.SetInt("_IsSelected", 1);
    }
    public void OnDeSelected()
    {
        m_Material.SetInt("_IsSelected", 0);
    }
}
