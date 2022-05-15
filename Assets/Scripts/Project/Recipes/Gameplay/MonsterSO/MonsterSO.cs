using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterFriendliness
{
    Friendly,
    Enemy
}

public enum MonsterCategory
{
    Melee,
    Ranged,
    Healer,
    Defender
}

[CreateAssetMenu(fileName = "New Monster", menuName = "Custom Data/Monsters/Monster")]
public class MonsterSO : BaseIngredientSO
{
    public Sprite monsterIcon;

    public MonsterFriendliness monsterFriendliness;
    public MonsterCategory category;

    // Stats
    public float health = 0;
    public float attack = 0;
    public float defense = 0;
    public float immunity = 0;
    
    public GameObject prefab;
}
