using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
{
    public int healthAmount;
    public override void ExecuteEffect(UsableItem parentItem, Damageable character)
    {
        character.Health += healthAmount;
        HealthBar.barInstance.SetHealth(character.Health);
    }

    public override void ExecuteEffect2(UsableItem parentItem, Character character)
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
