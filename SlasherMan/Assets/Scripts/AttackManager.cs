using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    private Ability support;
    private Ability attack;
    private Ability special;

    [Header("UILinks")]
    public Image supportImage;
    public Image specialImage;
    public Image attackImage;

    public InventoryManager inventoryManager;

    public List<Animator> supportTokenAnimators = new List<Animator>();
    public List<Animator> specialTokenAnimators = new List<Animator>();

    private bool attackBlock = false;
    public bool isAttackBlocked() { return attackBlock; }

    public Ability[] getAbilities()
    {
        Ability[] abs = { attack, support, special };
        return abs;
    }

    private void Start()
    {
        init();
    }

    public void init()
    {
        /*
        dash = transform.Find("DashManager").GetComponent<DashAttack>();
        slash = transform.Find("SlashManager").GetComponent<SlashAttack>();
        blaze = transform.Find("BlazeManager").GetComponent<BlazeAttack>();
        */
        //BlazeImage.color = Color.red;
        //DashImage.color = Color.red;
        //SlashImage.color = Color.white;

        support.allowed = false;
        special.allowed = false;

        attackBlock = false;

        foreach (Animator a in supportTokenAnimators)
        {
            a.SetTrigger("Fade");
        }

        foreach (Animator a in specialTokenAnimators)
        {
            a.SetTrigger("Fade");
        }
    }

    public void updateAbility(PlayerItemScriptable abilityItem)
    {
        Ability a = abilityItem.instanciatedObject.GetComponent<Ability>();

        //Debug.log
        if(abilityItem.type == ItemType.attack)
        {
            attack = a;
        }
        else if(abilityItem.type == ItemType.support)
        {
            support = a;
        }
        else if(abilityItem.type == ItemType.special)
        {
            special = a;
        }
        else
        {
            Debug.LogWarning(abilityItem.type + " not supported in attack Manager");
        }
    }

    private void updateButtons()
    {
        PlayerItemScriptable item;

        //BlazeImage.color = colorFor(blaze.canBeUsed());
        if (inventoryManager.tryGetItemInSlot(ItemType.special, out item))
        {
            Sprite specialSprite = item.itemSpriteON;
            if (!special.canBeCasted() || attackBlock)
            {
                specialSprite = item.itemSpriteOFF;
            }
            specialImage.sprite = specialSprite;
        }

        if (inventoryManager.tryGetItemInSlot(ItemType.support, out item))
        {
            Sprite supportSprite = item.itemSpriteON;
            if (!support.canBeCasted() || attackBlock)
            {
                supportSprite = item.itemSpriteOFF;
            }
            supportImage.sprite = supportSprite;
        }

        if (inventoryManager.tryGetItemInSlot(ItemType.attack, out item))
        {
            Sprite attackSprite = item.itemSpriteON;
            if (!attack.canBeCasted() || attackBlock)
            {
                attackSprite = item.itemSpriteOFF;
            }
            this.attackImage.sprite = attackSprite;
        }
        //i am convinced there is a better way to do this, but still dunno how, will figure out later
    }

    private Color colorFor(bool b)
    {
        return attackBlock ? Color.grey : b ? Color.white : Color.red;
    }

    public void registerSpecial()
    {
        attackBlock = true;
        int oldAmount = special.chargeAmount;
        special.chargeAmount = Mathf.Max(0, special.chargeAmount - special.chargeCost);
        updateButtons();

        for(int i = oldAmount - 1; i >= special.chargeAmount; i--)
        {
            specialTokenAnimators[i].SetTrigger("fade");
        }
    }

    internal void preventAttack()
    {
        attackBlock = true;
    }

    private void Update()
    {
        updateButtons();
    }

    public void registerSupport()
    {
        attackBlock = true;
        int oldAmount = support.chargeAmount;
        support.chargeAmount = Mathf.Max(0, support.chargeAmount - support.chargeCost);

        for (int i = oldAmount - 1; i >= support.chargeAmount; i--)
        {
             supportTokenAnimators[i].SetTrigger("fade");
        }    
    }

    public void releaseAttackBlock()
    {
        attackBlock = false;
    }

    public void registerAttack()
    {
        attackBlock = true;

        if (support.chargeAmount < support.chargeCapacity)
        {
            supportTokenAnimators[support.chargeAmount].gameObject.SetActive(true);
            supportTokenAnimators[support.chargeAmount].SetTrigger("Pop");
            support.addCharge();
        }

        if (special.chargeAmount < special.chargeCost)
        {
            specialTokenAnimators[special.chargeAmount].gameObject.SetActive(true);
            specialTokenAnimators[special.chargeAmount].SetTrigger("Pop");
            special.chargeAmount++;
        }
    }   
}
