using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayer : MonoBehaviour
{
    public List<InventorySlotBehaviour> slots = new List<InventorySlotBehaviour>();
    public InventoryManager inventoryManager;

    public GameObject lootPrefab;
    public Transform lootHolder;

    private GameObject loot = null;

    public float nonPertinentSlotAlpha = 0.1f;

    public void refreshSlots()
    {
        foreach (InventorySlotBehaviour slot in slots)
        {
            inventoryManager.tryGetItemInSlot(slot.associatedDisplay.specificType, out PlayerItemScriptable item);
            slot.associatedDisplay.setItem(item);
        }
    }

    public void notifyStartDrag(ItemType itemType)
    {
        foreach(InventorySlotBehaviour slot in slots)
        {
            if(slot.associatedDisplay.specificType != itemType)
            {
                slot.canvasGroup.alpha = nonPertinentSlotAlpha;
            }
        }
    }

    public void notifyEndDrag()
    {
        foreach (InventorySlotBehaviour slot in slots)
        {
            slot.canvasGroup.alpha = 1;        
        }
    }

    public void generateALoot(int stageNumber)
    {
        PlayerItemScriptable lootItem = inventoryManager.getRandomItem(stageNumber);
        ItemLootBehaviour lootBehaviour = Instantiate(lootPrefab, lootHolder).GetComponent<ItemLootBehaviour>();
        PlayerItemDisplayer displayer = lootBehaviour.GetComponent<PlayerItemDisplayer>();
        displayer.setItem(lootItem);
        lootBehaviour.inventoryDisplayer = this;

        loot = lootBehaviour.gameObject;
    }

    internal void removeLootIfLeft()
    {
        if(loot != null)
        {
            Destroy(loot);
        }
    }
}
