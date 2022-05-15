using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeMonsterProgress : MonoBehaviour
{
    [SerializeField]
    private Slider m_Progressbar = null;

    private void OnEnable()
    {
        CauldronManager.OnMonsterProgress += OnMonsterProgress;
    }
    private void OnDisable()
    {
        CauldronManager.OnMonsterProgress -= OnMonsterProgress;
    }

    private void OnDestroy()
    {
        CauldronManager.OnMonsterProgress -= OnMonsterProgress;
    }

    private void OnMonsterProgress(float progress)
    {
        m_Progressbar.value = progress;
    }
}
