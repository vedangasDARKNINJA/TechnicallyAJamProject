using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronObject : MonoBehaviour, IPickableObject
{
    /*
     * QUICK AND DIRTY
     */
    public Color notSelectedColor = Color.green;
    public Color selectedColor = Color.red;

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
        m_Material.color = selectedColor;
    }
    public void OnDeSelected()
    {
        m_Material.color = notSelectedColor;
    }
}
