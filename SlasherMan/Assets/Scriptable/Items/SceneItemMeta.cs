using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class SceneItemMeta : ScriptableObject
{
    public GameObject prefab;
    public float probability = 1;
    public string itemName;

    public bool canMoveInCell = false;
}
