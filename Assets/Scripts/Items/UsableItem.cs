using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public override string GetItemType()
    {
        return IsConsumable ? "Consumable" : "Usable";
    }

    public override string GetDescription()
    {
        sb.Length = 0;

        foreach (UsableItemEffect effect in Effects)
        {
            sb.AppendLine(effect.GetDescription());
        
        }
        foreach (UsableItemEffect buff in Buff)
        {
            sb.AppendLine(buff.GetDescription());
        }

        return sb.ToString();
    }
}
