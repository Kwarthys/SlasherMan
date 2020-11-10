using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { attack, support, special, body, foot}

[CreateAssetMenu(fileName = "PlayerItem", menuName = "PlayerItem")]
public class PlayerItemScriptable : ScriptableObject
{
    public Sprite itemSpriteON;
    public Sprite itemSpriteOFF;
    public ItemType type;
    public GameObject prefab;
    public GameObject instanciatedObject = null;
}
