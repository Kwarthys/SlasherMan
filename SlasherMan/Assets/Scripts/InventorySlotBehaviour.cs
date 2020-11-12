using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotBehaviour : MonoBehaviour, IDropHandler
{
    public PlayerItemDisplayer associatedDisplay;
    public CanvasGroup canvasGroup;

    public InventoryManager inventoryManager;

    public InventoryDisplayer inventoryDisplayer;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("Dropped in this slot !");
        GameObject dragged = eventData.pointerDrag;

        if(dragged != null)
        {
            ItemLootBehaviour loot = dragged.GetComponent<ItemLootBehaviour>();
            if(loot != null)
            {
                //Debug.Log(loot.associatedItem.itemName);
                if(associatedDisplay.setItem(loot.associatedItem))
                {
                    //association successful
                    inventoryManager.replaceItem(loot.associatedItem);
                    Destroy(dragged);
                }
            }

        }

        inventoryDisplayer.notifyEndDrag();
    }
}
