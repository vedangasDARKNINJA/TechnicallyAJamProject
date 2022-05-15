using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngredientObject : MonoBehaviour
{
    public IngredientIdentifierSO identifier;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(identifier)
        {
            gameObject.name = identifier.ingredientName;
        }
    }
#endif

}
