using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    private Ability support = null;
    private Ability attack = null;
    private Ability special = null;

    [Header("UILinks")]
    public Image supportImage;
    public Image specialImage;
    public Image attackImage;

    [Space]
    public int bonusDamage = 0;
    public float bonusDamageCoef = 1;

    public InventoryManager inventoryManager;

    public List<Animator> supportTokenAnimators = new List<Animator>();
    public List<Animator> specialTokenAnimators = new List<Animator>();

    public bool masterAttackBlock = false; //used by managers

    [SerializeField]
    private bool attackBlock = false; //used by abilities
    public bool isAttackBlocked() { return attackBlock || masterAttackBlock; }

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

        attackBlock = false;

        foreach (Animator a in supportTokenAnimators)
        {
            a.SetTrigger("Fade");
        }

        foreach (Animator a in specialTokenAnimators)
        {
            a.SetTrigger("Fade");
        }

        attack.chargeAmount = 0;

        if(support != null)
            support.chargeAmount = 0;
        if(special != null)
            special.chargeAmount = 0;
    }

    public void updateAbility(PlayerItem abilityItem)
    {
        Ability a = abilityItem.instanciatedEffector.GetComponent<Ability>();
        a.level = abilityItem.itemLevel;

        //Debug.log
        if(abilityItem.type == ItemType.Attack)
        {
            attack = a;
        }
        else if(abilityItem.type == ItemType.Support)
        {
            support = a;
        }
        else if(abilityItem.type == ItemType.Special)
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
        PlayerItem item;

        if (inventoryManager.tryGetItemInSlot(ItemType.Special, out item))
        {
            Sprite specialSprite = item.baseScriptable.itemSpriteON;
            if (!special.canBeCasted() || attackBlock)
            {
                specialSprite = item.baseScriptable.itemSpriteOFF;
            }
            specialImage.sprite = specialSprite;
        }

        if (inventoryManager.tryGetItemInSlot(ItemType.Support, out item))
        {
            Sprite supportSprite = item.baseScriptable.itemSpriteON;
            if (!support.canBeCasted() || attackBlock)
            {
                supportSprite = item.baseScriptable.itemSpriteOFF;
            }
            supportImage.sprite = supportSprite;
        }

        if (inventoryManager.tryGetItemInSlot(ItemType.Attack, out item))
        {
            Sprite attackSprite = item.baseScriptable.itemSpriteON;
            if (!attack.canBeCasted() || attackBlock)
            {
                attackSprite = item.baseScriptable.itemSpriteOFF;
            }
            attackImage.sprite = attackSprite;
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
            specialTokenAnimators[i].SetTrigger("Fade");
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
             supportTokenAnimators[i].SetTrigger("Fade");
        }    
    }

    public void releaseAttackBlock()
    {
        attackBlock = false;
    }

    public void registerAddCharge()
    {
        if (support != null)
        {
            if (support.chargeAmount < support.chargeCapacity)
            {
                supportTokenAnimators[support.chargeAmount].gameObject.SetActive(true);
                supportTokenAnimators[support.chargeAmount].SetTrigger("Pop");
                support.addCharge();
            }
        }

        if (special != null)
        {
            if (special.chargeAmount < special.chargeCost)
            {
                specialTokenAnimators[special.chargeAmount].gameObject.SetActive(true);
                specialTokenAnimators[special.chargeAmount].SetTrigger("Pop");
                special.addCharge();
            }
        }
    }

    public void registerAttack()
    {
        attackBlock = true;

        /*
        if(support != null)
        {
            if (support.chargeAmount < support.chargeCapacity)
            {
                supportTokenAnimators[support.chargeAmount].gameObject.SetActive(true);
                supportTokenAnimators[support.chargeAmount].SetTrigger("Pop");
                support.addCharge();
            }
        }

        if(special != null)
        {
            if (special.chargeAmount < special.chargeCost)
            {
                specialTokenAnimators[special.chargeAmount].gameObject.SetActive(true);
                specialTokenAnimators[special.chargeAmount].SetTrigger("Pop");
                special.chargeAmount++;
            }
        }
        */
    }   
}
