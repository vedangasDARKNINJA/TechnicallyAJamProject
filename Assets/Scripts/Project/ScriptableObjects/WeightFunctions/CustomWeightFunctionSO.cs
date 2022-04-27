using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equal Weight Function", menuName = "Custom Data/Interaction System/Weight Function")]
public class CustomWeightFunctionSO : DetectorWeightFunctionSO
{
    [Range(0.0f, 1.0f)]
    public float distanceWeight = 0.0f;

    [Range(0.0f, 1.0f)]
    public float angleWeight = 0.0f;

    public override float CalculateWeight(float distance, float angle)
    {
        return distanceWeight * distance + angleWeight * angle;
    }
}
