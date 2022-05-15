using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDataManager : MonoBehaviour
{
    public static RecipeDataManager instance = null;

    public RecipeSO[] recipes;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
