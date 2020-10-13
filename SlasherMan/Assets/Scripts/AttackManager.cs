using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    private DashAttack dash;
    private SlashAttack slash;
    private BlazeAttack blaze;

    private int slashCount = 0;
    private int dashCount = 0;

    public int blazeSlashCost = 4;
    public int dashMaxStock = 2;

    public Image DashImage;
    public Image BlazeImage;
    public Image SlashImage;

    private bool attackBlock = false;
    public bool isAttackBlocked() { return attackBlock; }

    private void Start()
    {
        init();
    }

    public void init()
    {
        dash = transform.Find("DashManager").GetComponent<DashAttack>();
        slash = transform.Find("SlashManager").GetComponent<SlashAttack>();
        blaze = transform.Find("BlazeManager").GetComponent<BlazeAttack>();
        BlazeImage.color = Color.red;
        DashImage.color = Color.red;
        SlashImage.color = Color.white;

        slashCount = 0;
        dashCount = 0;

        dash.allowed = false;
        blaze.allowed = false;
    }

    private void updateButtons()
    {
        BlazeImage.color = colorFor(blaze.canBeUsed());
        DashImage.color = colorFor(dash.canBeUsed());
        SlashImage.color = colorFor(slash.canBeUsed());
    }

    private Color colorFor(bool b)
    {
        return attackBlock ? Color.grey : b ? Color.white : Color.red;
    }

    public void registerBlaze()
    {
        slashCount = 0;
        attackBlock = true;
        blaze.allowed = false;
        updateButtons();
    }

    private void Update()
    {
        updateButtons();
    }

    public void registerDash()
    {
        attackBlock = true;
        dashCount = Mathf.Max(0, dashCount - 1);
        if(dashCount == 0)
        {
            dash.allowed = false;
        }
    }

    public void releaseAttackBlock()
    {
        attackBlock = false;
    }

    public void registerSlash()
    {
        attackBlock = true;

        slashCount = Mathf.Min(blazeSlashCost, slashCount + 1);
        dashCount = Mathf.Min(dashMaxStock, dashCount+1);

        dash.allowed = true;

        if (slashCount == blazeSlashCost)
        {
            blaze.allowed = true;
        }
    }
}
