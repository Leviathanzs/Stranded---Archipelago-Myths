using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Accessory1,
    Accessory2,
    Helmet,
    Chest,
    Gloves,
    Boots,
    Weapon,
    SubWeapon,
}

[CreateAssetMenu(menuName = "Items/Equippable Item")]
public class EquippableItem : Item
{
    public int StrenghtBonus;
    public int AgilityBonus;
    public int IntelligenceBonus;
    public int VitalityBonus;
    [Space]
    public float StrenghtPercentBonus;
    public float AgilityPercentBonus;
    public float IntelligencePercentBonus;
    public float VitalityPercentBonus;
    [Space]
    public EquipmentType EquipmentType;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

    public void Equip(Character c)
    {
        if(StrenghtBonus != 0)
            c.Strenght.AddModifier(new StatModifier(StrenghtBonus, StatModType.Flat, this));
        if(AgilityBonus != 0)
            c.Agility.AddModifier(new StatModifier(AgilityBonus, StatModType.Flat, this));
        if(IntelligenceBonus != 0)
            c.Intelligence.AddModifier(new StatModifier(IntelligenceBonus, StatModType.Flat, this));
        if(VitalityBonus != 0)
            c.Vitality.AddModifier(new StatModifier(VitalityBonus, StatModType.Flat, this));

        if(StrenghtPercentBonus != 0)
            c.Strenght.AddModifier(new StatModifier(StrenghtPercentBonus, StatModType.PercentMult, this));
        if(AgilityPercentBonus != 0)
            c.Agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModType.PercentMult, this));
        if(IntelligencePercentBonus != 0)
            c.Intelligence.AddModifier(new StatModifier(IntelligencePercentBonus, StatModType.PercentMult, this));
        if(VitalityPercentBonus != 0)
            c.Vitality.AddModifier(new StatModifier(VitalityPercentBonus, StatModType.PercentMult, this));
    }

    public void Unequip(Character c)
    {
        c.Strenght.RemoveAllModifiersFromSource(this);
        c.Agility.RemoveAllModifiersFromSource(this);
        c.Intelligence.RemoveAllModifiersFromSource(this);
        c.Vitality.RemoveAllModifiersFromSource(this);
    }

    public override string GetItemType()
    {
        return EquipmentType.ToString();
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        AddStat(StrenghtBonus, "Strenght");
        AddStat(AgilityBonus, "Agility");
        AddStat(IntelligenceBonus, "Intelligence");
        AddStat(VitalityBonus, "Vitality");

        AddStat(StrenghtPercentBonus, "Strenght", true);
        AddStat(AgilityPercentBonus, "Agility", true);
        AddStat(IntelligencePercentBonus, "Intelligence", true);
        AddStat(VitalityPercentBonus, "Vitality", true);


        return sb.ToString();
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if(value != 0)
        {
            if(sb.Length > 0)
                sb.AppendLine();

            if(value > 0)
                sb.Append("+");

            if(isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }
            sb.Append(statName);
        }
    }
}
