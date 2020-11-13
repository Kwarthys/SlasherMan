using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType { Armor, Speed, FlatDamage, PercentDamage, Life}

public abstract class ItemModifierEffect
{
    public float finalAmount = 0;
    public float amount;
    public float levelCoef;
    public ModifierType modifierType;

    public string modifierEffect;
    public string modifierItemName;

    protected int level;

    public abstract void applyEffects(PlayerController controller, PlayerHealth health, AttackManager attacks);
    public abstract void removeEffects(PlayerController controller, PlayerHealth health, AttackManager attacks);

    protected void computeEffect()
    {
        finalAmount = Mathf.RoundToInt(amount * Mathf.Pow(levelCoef, level));
        //Debug.Log(modifierEffect + "(" + level + ") " + amount + "->" + finalAmount + " modified by " + Mathf.Pow(levelCoef, level));
    }

    public static ItemModifierEffect getRandomModifierOfLevel(int level)
    {
        if(Random.value > 0.95)
        {
            level++; //Rare quality item
        }

        int rand = Random.Range(0, 100);

        ItemModifierEffect modifier;

        if (rand < 25)
        {
            modifier = new ArmourModifier(level);
        }
        else if (rand < 50)
        {
            modifier = new SpeedModifier(level);
        }
        else if (rand < 75)
        {
            modifier = new FlatDamageModifier(level);
        }
        else
        {
            modifier = new ArmourModifier(level);
        }

        return modifier;
    }
}

public class ArmourModifier : ItemModifierEffect
{
    public override void applyEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        health.bonusArmor += (int)finalAmount;
    }

    public override void removeEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        health.bonusArmor -= (int)finalAmount;
    }

    public ArmourModifier(int level)
    {
        levelCoef = 1.1f;
        modifierType = ModifierType.Armor;
        modifierEffect = "Of Armor";
        modifierItemName = " of the Defenser";
        amount = 2;

        this.level = level;

        computeEffect();
    }
}

public class SpeedModifier : ItemModifierEffect
{
    public override void applyEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        controller.bonusSpeedCoef += finalAmount/100;
    }

    public override void removeEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        controller.bonusSpeedCoef -= finalAmount/100;
    }

    public SpeedModifier(int level)
    {
        levelCoef = 1.05f;
        modifierType = ModifierType.Speed;
        modifierEffect = "% Of Speed";
        modifierItemName = " of Swiftness";
        amount = 5;

        this.level = level;

        computeEffect();
    }
}

public class FlatDamageModifier : ItemModifierEffect
{
    public override void applyEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        attacks.bonusDamage += (int)finalAmount;
    }

    public override void removeEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        attacks.bonusDamage -= (int)finalAmount;
    }

    public FlatDamageModifier(int level)
    {
        levelCoef = 1.05f;
        modifierType = ModifierType.FlatDamage;
        modifierEffect = "Of Damage";
        modifierItemName = " of Rage";
        amount = 5;

        this.level = level;

        computeEffect();
    }
}
