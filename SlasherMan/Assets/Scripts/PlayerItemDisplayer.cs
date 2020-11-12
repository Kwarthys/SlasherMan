using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItemDisplayer : MonoBehaviour
{
    public PlayerItemScriptable item;
    public TextMeshProUGUI itemNameText;

    public bool isSpecific = false;
    public ItemType specificType;

    private Image image;

    public bool setItem(PlayerItemScriptable item)
    {
        if(isSpecific && item!=null)
        {
            if(item.type != specificType)
            {
                //Debug.LogError("Wrong type on wrong slot");
                return false;
            }
        }

        this.item = item;
        refreshUI();
        return true;
    }

    public void refreshUI()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (item == null)
        {
            image.sprite = null;
            image.color = new Color(1, 1, 1, 0.1f);
            if(isSpecific)
            {
                itemNameText.text = specificType + " slot";
            }
            else
            {
                itemNameText.text = "Empty";
            }
        }
        else
        {
            image.color = new Color(1, 1, 1);
            image.sprite = item.itemSpriteON;
            itemNameText.text = item.itemName + " (" + item.itemLevel + ")";
        }
    }

    private void OnValidate()
    {
        if(itemNameText != null)
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }

            refreshUI();
        }
    }
}
