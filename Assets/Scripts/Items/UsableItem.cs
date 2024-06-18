using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Usable Item")]
public class UsableItem : Item
{
    public bool IsConsumable;
    public List<UsableItemEffect> Effects;
    public List<UsableItemEffect> Buff;

    public virtual void Use(Damageable character)
    {
        foreach(UsableItemEffect effect in Effects)
        {
            effect.ExecuteEffect(this, character);
        }
    }
    public virtual void Use2(Character character)
    {
        foreach(UsableItemEffect buff in Buff)
        {
            buff.ExecuteEffect2(this, character);
        }
    }
}
