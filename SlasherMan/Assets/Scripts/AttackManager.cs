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
    /*
    private DashAttack support;
    private SlashAttack attack;
    private BlazeAttack special;
    */
    private int attackCount = 0;
    private int supportCharges = 0;

    public int specialCost = 4;
    public int supportMaxCharges = 2;

    [Header("UILinks")]
    public Image supportImage;
    public Image specialImage;
    public Image attackImage;

    public Sprite specialON;
    public Sprite specialOFF;

    public Sprite supportON;
    public Sprite supportOFF;

    public Sprite attackON;
    public Sprite attackOFF;

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

        attackCount = 0;
        supportCharges = 0;

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

    public void updateAbility(ItemType slot, GameObject ability)
    {
        Ability a = ability.GetComponent<Ability>();

        //Debug.log
        if(slot == ItemType.attack)
        {
            attack = a;
        }
        else if(slot == ItemType.support)
        {
            support = a;
        }
        else if(slot == ItemType.special)
        {
            special = a;
        }
        else
        {
            Debug.LogWarning(slot + " not supported in attack Manager");
        }
    }

    private void updateButtons()
    {
        //BlazeImage.color = colorFor(blaze.canBeUsed());
        if(special != null)
        {
            Sprite specialSprite = specialON;
            if (!special.canBeCasted() || attackBlock)
            {
                specialSprite = specialOFF;
            }
            specialImage.sprite = specialSprite;
        }

        if(support!=null)
        {
            Sprite supportSprite = supportON;
            if (!support.canBeCasted() || attackBlock)
            {
                supportSprite = supportOFF;
            }
            supportImage.sprite = supportSprite;
        }

        if(attack != null)
        {
            Sprite attackSprite = attackON;
            if (!attack.canBeCasted() || attackBlock)
            {
                attackSprite = attackOFF;
            }
            this.attackImage.sprite = attackSprite;
        }

    }

    private Color colorFor(bool b)
    {
        return attackBlock ? Color.grey : b ? Color.white : Color.red;
    }

    public void registerBlaze()
    {
        attackCount = 0;
        attackBlock = true;
        special.allowed = false;
        updateButtons();

        foreach(Animator a in specialTokenAnimators)
        {
            a.SetTrigger("Fade");
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

    public void registerDash()
    {
        attackBlock = true;
        supportCharges = Mathf.Max(0, supportCharges - 1);

        supportTokenAnimators[supportCharges].SetTrigger("Fade");

        if (supportCharges == 0)
        {
            support.allowed = false;
        }
    }

    public void releaseAttackBlock()
    {
        attackBlock = false;
    }

    public void registerSlash()
    {
        attackBlock = true;

        if (supportCharges < supportMaxCharges)
        {
            supportTokenAnimators[supportCharges].gameObject.SetActive(true);
            supportTokenAnimators[supportCharges].SetTrigger("Pop");
        }

        if (attackCount < specialCost)
        {
            specialTokenAnimators[attackCount].gameObject.SetActive(true);
            specialTokenAnimators[attackCount].SetTrigger("Pop");
        }

        attackCount = Mathf.Min(specialCost, attackCount + 1);
        supportCharges = Mathf.Min(supportMaxCharges, supportCharges+1);

        support.allowed = true;

        if (attackCount == specialCost)
        {
            special.allowed = true;
        }
    }
}
