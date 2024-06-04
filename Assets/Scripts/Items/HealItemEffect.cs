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
    }
}
