using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct RecipeStatRowData
{
    public static RecipeStatRowData ToRecipeRowData<T>(string name,T data)
    {
        return new RecipeStatRowData { statFieldName = name, statFieldValue = data.ToString() };
    }

    public string statFieldName;
    public string statFieldValue;
}

public class RecipeStatRow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_TextName = null;

    [SerializeField]
    private TextMeshProUGUI m_TextValue = null;

    public void SetData(RecipeStatRowData data)
    {
        gameObject.SetActive(true);
        m_TextName.text = data.statFieldName;
        m_TextValue.text = data.statFieldValue;
    }

    public void SetData(string name,string value)
    {
        SetData(new RecipeStatRowData{ statFieldName=name,statFieldValue=value});
    }
}
