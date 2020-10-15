using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Monster")]
public class MonsterMeta : ScriptableObject
{
    public GameObject monsterPrefab;
    public string monsterName;
    public int monsterCost;
    public float spawnChances;
}
