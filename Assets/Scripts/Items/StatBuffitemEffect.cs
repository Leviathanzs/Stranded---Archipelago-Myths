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

        // Recalculate stats and update max health
        character.RecalculateBaseStats();
        character.UpdateHealthAfterBuff();

        // Start coroutine to remove the buff after duration
        CoroutineManager.Instance.RunCoroutine(RemoveBuff(character, statModifier, Duration));
        character.UpdateStatValues();
    }

    public override string GetDescription()
    {
        return "Grants " + StrenghtBuff + " Strength For " + Duration + " Seconds.";
    }

    public static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Remove strength modifier and recalculate stats
        character.Strenght.RemoveModifier(statModifier);
        character.RecalculateBaseStats();
        character.UpdateHealthAfterBuff();
        character.UpdateStatValues();
    }

}
