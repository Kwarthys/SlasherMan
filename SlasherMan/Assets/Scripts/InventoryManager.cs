using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerItem
{
    public PlayerItemScriptable baseScriptable;
    public int itemLevel = 1;
    public ItemType type;
    public GameObject instanciatedEffector = null;

    private List<ItemModifierEffect> modifiers = new List<ItemModifierEffect>();

    public PlayerItem(PlayerItemScriptable baseItem, int level = 1)
    {
        baseScriptable = baseItem;
        itemLevel = level;
        type = baseItem.type;
    }

    public List<ItemModifierEffect> getModifiers() { return modifiers; }

    public void addModifier(ItemModifierEffect modifier)
    {
        ItemModifierEffect same = null;
        bool found = false;
        for(int i = 0; i < modifiers.Count && !found; ++i)
        {
            if(modifiers[i].modifierType == modifier.modifierType)
            {
                found = true;
                same = modifiers[i];
            }
        }

        if(found)
        {
            same.finalAmount += modifier.finalAmount;
        }
        else
        {
            modifiers.Add(modifier);
        }
    }
}


public class InventoryManager : MonoBehaviour
{
    public List<PlayerItemScriptable> starterItems = new List<PlayerItemScriptable>();

    public List<PlayerItemScriptable> allItemsInGame = new List<PlayerItemScriptable>();

    public Dictionary<ItemType, PlayerItem> equippedItems = new Dictionary<ItemType, PlayerItem>();

    public Image attackImage;
    public Image supportImage;
    public Image specialImage;

    public Transform playerAbilityHolder;

    public AttackManager attackManager;
    public PlayerController controller;
    public PlayerHealth health;

    private void Awake()
    {
        foreach (PlayerItemScriptable item in starterItems)
        {
            PlayerItem instanciatedObect = new PlayerItem(item);

            if (equippedItems.ContainsKey(item.type))
            {
                Debug.LogWarning("Replacing item in starter");
                if(isItemWeapon(item.type))
                {
                    Destroy(equippedItems[item.type].instanciatedEffector);
                    equippedItems[item.type].instanciatedEffector = null;
                    equippedItems[item.type] = instanciatedObect;
                    equippedItems[item.type].instanciatedEffector = Instantiate(item.prefab, playerAbilityHolder);
                    attackManager.updateAbility(instanciatedObect);
                }
                else
                {
                    equippedItems[item.type] = instanciatedObect;
                }
            }
            else
            {
                equippedItems.Add(item.type, instanciatedObect);
                if (isItemWeapon(item.type))
                {
                    instanciatedObect.instanciatedEffector = Instantiate(item.prefab, playerAbilityHolder);
                    attackManager.updateAbility(instanciatedObect);
                }
            }
        }

        updateUI();
    }

    private bool isItemWeapon(ItemType type)
    {
        return type == ItemType.Attack || type == ItemType.Support || type == ItemType.Special;
    }

    public void replaceItem(PlayerItem newItem)
    {
        if(equippedItems.ContainsKey(newItem.type))
        {
            foreach(ItemModifierEffect effect in equippedItems[newItem.type].getModifiers())
            {
                effect.removeEffects(controller, health, attackManager);
            }

            if (isItemWeapon(newItem.type))
            {
                Destroy(equippedItems[newItem.type].instanciatedEffector);
                equippedItems[newItem.type].instanciatedEffector = null;
            }
        }

        equippedItems[newItem.type] = newItem;

        foreach (ItemModifierEffect effect in equippedItems[newItem.type].getModifiers())
        {
            effect.applyEffects(controller, health, attackManager);
        }

        if (isItemWeapon(newItem.type))
        {
            equippedItems[newItem.type].instanciatedEffector = Instantiate(newItem.baseScriptable.prefab, playerAbilityHolder);
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

    public bool tryGetItemInSlot(ItemType slot, out PlayerItem item)
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
        if (tryGetItemInSlot(slot, out PlayerItem item))
        {
            image.sprite = item.baseScriptable.itemSpriteON;
        }
        else
        {
            image.sprite = null;
        }
    }

    public PlayerItem getRandomItem(int stageNumber)
    {
        int r = UnityEngine.Random.Range(0, allItemsInGame.Count);
        PlayerItem item = new PlayerItem(allItemsInGame[r], stageNumber);

        int modifCap = UnityEngine.Random.Range(1, Mathf.Min(4, stageNumber));

        for(int i = 0; i < modifCap; ++i)
        {
            item.addModifier(ItemModifierEffect.getRandomModifierOfLevel(stageNumber - i));
        }

        return item;
    }
}
