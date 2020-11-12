using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Attack, Support, Special, Crown, Foot, Body}

[CreateAssetMenu(fileName = "PlayerItem", menuName = "PlayerItem")]
public class PlayerItemScriptable : ScriptableObject
{
    public Sprite itemSpriteON;
    public Sprite itemSpriteOFF;
    public ItemType type;
    public GameObject prefab;
    public GameObject instanciatedObject = null;

    public string itemName =  "Item";

    public int itemLevel = 1;
}
