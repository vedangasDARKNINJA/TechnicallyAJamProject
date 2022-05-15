using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeIngredientUI : MonoBehaviour
{
    [SerializeField]
    private Image m_Image;

    [SerializeField]
    private TextMeshProUGUI m_Text;

    public void SetData(Sprite icon, uint quantity)
    {
        if(m_Image!=null)
        {
            m_Image.sprite = icon;
        }
        else
        {
            Debug.LogError("RecipeIngredientUI Icon not set");
        }

        if (m_Text != null)
        {
            m_Text.text = $"x{quantity}";
        }
        else
        {
            Debug.LogError("RecipeIngredientUI Text not set");
        }
    }
    public void SetQuantity(int quantity)
    {
        if(quantity == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            m_Text.transform.DOScale(1.0f, 0.15f)
                .From(0.5f)
                .SetEase(Ease.OutExpo);

            m_Text.text = $"x{quantity}";
        }
    }
}
