using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Settings",menuName ="Custom Data/Player/Player Settings")]
public class PlayerSettingsSO : ScriptableObject
{
    public float moveSpeed = 0.0f;
    public float rotationSpeed= 0.0f;
}
