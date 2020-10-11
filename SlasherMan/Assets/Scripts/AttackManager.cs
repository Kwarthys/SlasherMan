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

    public void registerBlaze()
    {
        blaze.allowed = false;
        BlazeImage.color = Color.red;
    }

    private void Update()
    {
        SlashImage.color = slash.canBeUsed() ? Color.white : Color.red;
    }

    public void registerDash()
    {
        dashCount = Mathf.Max(0, dashCount - 1);
        if(dashCount == 0)
        {
            dash.allowed = false;
            DashImage.color = Color.red;
        }
    }

    public void registerSlash()
    {
        slashCount = Mathf.Min(blazeSlashCost, slashCount + 1);
        dashCount = Mathf.Min(dashMaxStock, dashCount+1);

        dash.allowed = true;
        DashImage.color = Color.white;

        if (slashCount == blazeSlashCost)
        {
            blaze.allowed = true;
            BlazeImage.color = Color.white;
            slashCount = 0;
        }
    }
}
