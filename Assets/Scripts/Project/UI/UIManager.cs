using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public WorldWidgetLayer worldWidgetLayer { get; private set; }

    private void Awake()
    {
        if(instance != this)
        {
            instance = this;
        }
        worldWidgetLayer = GetComponentInChildren<WorldWidgetLayer>();
    }
}
