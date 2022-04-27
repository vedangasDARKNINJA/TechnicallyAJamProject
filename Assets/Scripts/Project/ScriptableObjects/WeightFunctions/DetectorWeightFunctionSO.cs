using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetectorWeightFunctionSO : ScriptableObject
{
    public abstract float CalculateWeight(float distance, float angle);
}