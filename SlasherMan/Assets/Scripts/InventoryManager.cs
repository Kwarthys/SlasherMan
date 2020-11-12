using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<PlayerItemScriptable> starterItems = new List<PlayerItemScriptable>();

    public List<PlayerItemScriptable> allItemsInGame = new List<PlayerItemScriptable>();

    public Dictionary<ItemType, PlayerItemScriptable> equippedItems = new Dictionary<ItemType, PlayerItemScriptable>();

    public Image attackImage;
    public Image supportImage;
    public Image specialImage;

    public Transform playerAbilityHolder;

    public AttackManager attackManager;

    private void Awake()
    {
        foreach(PlayerItemScriptable item in starterItems)
        {
            if(equippedItems.ContainsKey(item.type))
            {
                Debug.LogWarning("Replacing item in starter");
                if(isItemWeapon(item.type))
                {
                    Destroy(equippedItems[item.type].instanciatedObject);
                    equippedItems[item.type].instanciatedObject = null;
                    equippedItems[item.type] = item;
                    equippedItems[item.type].instanciatedObject = Instantiate(item.prefab, playerAbilityHolder);
                    attackManager.updateAbility(item);
                }
                else
                {
                    equippedItems[item.type] = item;
                }
            }
            else
            {
                equippedItems.Add(item.type, item);
                if (isItemWeapon(item.type))
                {
                    item.instanciatedObject = Instantiate(item.prefab, playerAbilityHolder);
                    attackManager.updateAbility(item);
                }
            }
        }

        updateUI();
    }

    private bool isItemWeapon(ItemType type)
    {
        return type == ItemType.Attack || type == ItemType.Support || type == ItemType.Special;
    }

    public void replaceItem(PlayerItemScriptable newItem)
    {
        if (isItemWeapon(newItem.type))
        {
            if(equippedItems.ContainsKey(newItem.type))
            {
                Destroy(equippedItems[newItem.type].instanciatedObject);
                equippedItems[newItem.type].instanciatedObject = null;
            }
        }

        equippedItems[newItem.type] = newItem;

        if(isItemWeapon(newItem.type))
        {
            equippedItems[newItem.type].instanciatedObject = Instantiate(newItem.prefab, playerAbilityHolder);
            attackManager.updateAbility(newItem);
            updateUI();
        }
    }

    public void updateUI()
    {
        updateUIFor(ItemType.Attack, attackImage);
        updateUIFor(ItemType.Support, supportImage);
        updateUIFor(ItemType.Special, specialImage);
    }

    public bool tryGetItemInSlot(ItemType slot, out PlayerItemScriptable item)
    {
        item = null;

        if(equippedItems.ContainsKey(slot))
        {
            if(equippedItems[slot] != null)
            {
                item = equippedItems[slot];
                return true;
            }
        }

        return false;
    }

    private void updateUIFor(ItemType slot, Image image)
    {
        if (tryGetItemInSlot(slot, out PlayerItemScriptable item))
        {
            image.sprite = item.itemSpriteON;
        }
        else
        {
            image.sprite = null;
        }
    }


    public PlayerItemScriptable getRandomItem(int stageNumber)
    {
        int r = UnityEngine.Random.Range(0, allItemsInGame.Count);
        PlayerItemScriptable item = allItemsInGame[r];
        //item.itemLevel = stageNumber;
        return item;
    }
}
