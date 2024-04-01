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

[CreateAssetMenu]
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

}
