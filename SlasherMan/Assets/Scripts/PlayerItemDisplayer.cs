using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItemDisplayer : MonoBehaviour
{
    public PlayerItem item;
    public TextMeshProUGUI itemNameText;

    public bool isSpecific = false;
    public ItemType specificType;

    private Image image;

    public bool setItem(PlayerItem item)
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
            image.sprite = item.baseScriptable.itemSpriteON;
            itemNameText.text = item.baseScriptable.itemBaseName;

            if(item.getModifiers().Count > 0)
            {
                itemNameText.text += item.getModifiers()[0].modifierItemName;
            }

            itemNameText.text += " (" + item.itemLevel + ")<size=75%> ";

            foreach (ItemModifierEffect modifier in item.getModifiers())
            {
                itemNameText.text += "\n" + modifier.finalAmount + " " + modifier.modifierEffect;
            }
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
