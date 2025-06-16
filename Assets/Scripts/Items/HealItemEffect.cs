using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
{
    public int healthAmount;
    public AudioClip healSfx;
    public override void ExecuteEffect(UsableItem parentItem, Damageable character)
    {
        character.Health += healthAmount;
        HealthBar.barInstance.SetHealth(character.Health);

        if (healSfx != null)
        {
            AudioSource.PlayClipAtPoint(healSfx, character.transform.position);
        }
    }

    public override void ExecuteEffect2(UsableItem parentItem, Character character)
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        return "Heals for " + healthAmount + " health.";
    }
}
