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
        int rand = Random.Range(0, 100);

        ItemModifierEffect modifier;

        if (rand < 20)
        {
            modifier = new ArmourModifier(level);
        }
        else if (rand < 40)
        {
            modifier = new SpeedModifier(level);
        }
        else if (rand < 60)
        {
            modifier = new FlatDamageModifier(level);
        }
        else if (rand < 80)
        {
            modifier = new RelDamageModifier(level);
        }
        else
        {
            modifier = new LifeModifier(level);
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
        modifierItemName = " of the Defender";
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
        levelCoef = 1.01f;
        modifierType = ModifierType.Speed;
        modifierEffect = "% Of Speed";
        modifierItemName = " of Swiftness";
        amount = 2;

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
        amount = 2;

        this.level = level;

        computeEffect();
    }
}

public class RelDamageModifier : ItemModifierEffect
{
    public override void applyEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        attacks.bonusDamageCoef += (int)finalAmount;
    }

    public override void removeEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        attacks.bonusDamageCoef -= (int)finalAmount;
    }

    public RelDamageModifier(int level)
    {
        levelCoef = 1.05f;
        modifierType = ModifierType.FlatDamage;
        modifierEffect = "% Of Damage";
        modifierItemName = " of Hatred";
        amount = 5;

        this.level = level;

        computeEffect();
    }
}

public class LifeModifier : ItemModifierEffect
{
    public override void applyEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        health.changeBonusLife((int)finalAmount);
    }

    public override void removeEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        health.changeBonusLife(-(int)finalAmount);
    }

    public LifeModifier(int level)
    {
        levelCoef = 1.02f;
        modifierType = ModifierType.FlatDamage;
        modifierEffect = "Of Life";
        modifierItemName = " of Constitution";
        amount = 5;

        this.level = 1;

        computeEffect();
    }
}

public class TieModifier : ItemModifierEffect
{
    public override void applyEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        health.hasTie = true;
    }

    public override void removeEffects(PlayerController controller, PlayerHealth health, AttackManager attacks)
    {
        health.hasTie = false;
    }

    public TieModifier(int level)
    {
        levelCoef = 1;
        modifierType = ModifierType.FlatDamage;
        modifierEffect = "% Of freewill";
        modifierItemName = "";
        amount = -100;

        this.level = 0;

        computeEffect();
    }
}
