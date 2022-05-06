using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ObjectTypes
{
    None= 0,
    Cauldron = 1,
    Ingredient = 2,
    Monster = 4,
    Portal = 8
}
