using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Stat Buff")]
public class StatBuffitemEffect : UsableItemEffect
{
    public int StrenghtBuff;
    public float Duration;

    public override void ExecuteEffect(UsableItem parentItem, Damageable character)
    {
        throw new System.NotImplementedException();
    }

    public override void ExecuteEffect2(UsableItem parentItem, Character character)
    {
        StatModifier statModifier = new StatModifier(StrenghtBuff, StatModType.Flat, parentItem);
        character.Strenght.AddModifier(statModifier);
        character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
        character.UpdateStatValues();
    }

    public override string GetDescription()
    {
        return "Grants " + StrenghtBuff + " Strenght For " + Duration + " Seconds.";
    }

    public static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float Duration)
    {
        yield return new WaitForSeconds(Duration);
        character.Strenght.RemoveModifier(statModifier);
        character.UpdateStatValues();
    }
}
