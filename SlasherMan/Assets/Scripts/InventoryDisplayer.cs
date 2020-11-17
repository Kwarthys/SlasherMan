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
            inventoryManager.tryGetItemInSlot(slot.associatedDisplay.specificType, out PlayerItem item);
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

    private void activateLoot(PlayerItem item)
    {
        ItemLootBehaviour lootBehaviour = Instantiate(lootPrefab, lootHolder).GetComponent<ItemLootBehaviour>();
        PlayerItemDisplayer displayer = lootBehaviour.GetComponent<PlayerItemDisplayer>();
        displayer.setItem(item);
        lootBehaviour.inventoryDisplayer = this;

        loot = lootBehaviour.gameObject;
    }

    public void generateALoot(int stageNumber)
    {
        PlayerItem lootItem = inventoryManager.getRandomItem(stageNumber);

        activateLoot(lootItem);
    }

    public void removeLootIfLeft()
    {
        if(loot != null)
        {
            Destroy(loot);
        }
    }

    public void generateTieLoot()
    {
        activateLoot(inventoryManager.getDeadlyTie());
    }
}
