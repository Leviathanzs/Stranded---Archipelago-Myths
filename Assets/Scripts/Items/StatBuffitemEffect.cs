using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Stat Buff")]
public class StatBuffitemEffect : UsableItemEffect
{
    public int StrengthBuff;
    public float Duration;

    public override void ExecuteEffect(UsableItem parentItem, Damageable character)
    {
        throw new System.NotImplementedException();
    }

    public override void ExecuteEffect2(UsableItem parentItem, Character character)
    {
        // Check if the modifier for this item already exists
        StatModifier existingModifier = character.Strenght.GetModifierByItem(parentItem);

        if (existingModifier != null)
        {
            // If the modifier exists, reset its duration if the new one is longer
            if (existingModifier.Duration < Duration)
            {
                existingModifier.ResetDuration(Duration); // Reset the duration of the existing modifier
            }
        }
        else
        {
            // If no existing modifier, create a new one and add it
            // Use 0 as a default order (or set a custom order if necessary)
            StatModifier statModifier = new StatModifier(StrengthBuff, StatModType.Flat, 0, parentItem, Duration);
            character.Strenght.AddModifier(statModifier);

            // Start coroutine to remove the buff after the duration ends
            CoroutineManager.Instance.RunCoroutine(RemoveBuff(character, statModifier, Duration));
        }

        // Recalculate stats after applying the buff
        character.RecalculateBaseStats();
        character.UpdateHealthAfterBuff();
        character.UpdateStatValues();
    }

    public override string GetDescription()
    {
        return "Grants " + StrengthBuff + " Strength For " + Duration + " Seconds.";
    }

    // Coroutine to remove the buff after the specified duration
    public static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Remove the strength modifier and recalculate stats
        character.Strenght.RemoveModifier(statModifier);
        character.RecalculateBaseStats();
        character.UpdateHealthAfterBuff();
        character.UpdateStatValues();
    }
}