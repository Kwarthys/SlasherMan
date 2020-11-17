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

    public static InventoryManager inventoryManager;

    public int rarity = 0;

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

    /*
     * WHITE : 1 modifier 
     * GREEN : 2 mod
     * BLUE : 3 mod
     * RED : 4 mod
     * YELLOW : 4 upgraded mod
    */
    public static Color getItemColor(PlayerItem item)
    {
        Color c;
        if(item.rarity <= 1)
        {
            c = inventoryManager.rarity1;
        }
        else if(item.rarity == 2)
        {
            c = inventoryManager.rarity2;
        }
        else if(item.rarity == 3)
        {
            c = inventoryManager.rarity3;
        }
        else if(item.rarity == 4)
        {
            c = inventoryManager.rarity4;
        }
        else
        {
            c = inventoryManager.rarity5;
        }

        return c;
    }
}


public class InventoryManager : MonoBehaviour
{
    public List<PlayerItemScriptable> starterItems = new List<PlayerItemScriptable>();

    public List<PlayerItemScriptable> allItemsInGame = new List<PlayerItemScriptable>();

    public PlayerItemScriptable tieItem;

    public Dictionary<ItemType, PlayerItem> equippedItems = new Dictionary<ItemType, PlayerItem>();

    public Image attackImage;
    public Image supportImage;
    public Image specialImage;

    public Transform playerAbilityHolder;

    public AttackManager attackManager;
    public PlayerController controller;
    public PlayerHealth health;

    [Header("ItemColors")]
    public Color rarity1 = Color.white;
    public Color rarity2 = Color.green;
    public Color rarity3 = Color.blue;
    public Color rarity4 = Color.red;
    public Color rarity5 = Color.yellow;
    [Space]

    public Sprite emptySlot;

    private void Awake()
    {
        PlayerItem.inventoryManager = this;

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

    public void reinit()
    {
        equippedItems.Clear();
        Awake();
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
            image.sprite = emptySlot;
        }
    }

    public PlayerItem getDeadlyTie()
    {
        PlayerItem item = new PlayerItem(tieItem);
        item.rarity = 4;
        item.addModifier(new TieModifier(0));
        return item;
    }

    public PlayerItem getRandomItem(int stageNumber)
    {
        int r = UnityEngine.Random.Range(0, allItemsInGame.Count);
        PlayerItem item = new PlayerItem(allItemsInGame[r], stageNumber);

        int modifCap = UnityEngine.Random.Range(1, Mathf.Min(4, stageNumber));

        item.rarity = modifCap;

        for(int i = 0; i < modifCap; ++i)
        {
            int rarityModifier = 0;
            if(UnityEngine.Random.value > 0.99)
            {
                Debug.Log("WAow +1");
                rarityModifier = 1;
                item.rarity += 1;
            }

            item.addModifier(ItemModifierEffect.getRandomModifierOfLevel(stageNumber - i + rarityModifier));
        }

        return item;
    }
}
