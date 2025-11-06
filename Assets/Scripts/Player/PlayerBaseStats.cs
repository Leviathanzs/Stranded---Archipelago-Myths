using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System;

[Serializable]
public class PlayerBaseStats
{
    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public float BaseValue;
    protected float lastBasevalue = float.MinValue;
    protected bool isDirty = true;
    protected float _value;

    public virtual float Value {
        get 
        {
            if(isDirty || BaseValue != lastBasevalue) {
                lastBasevalue = BaseValue;
                _value = CalculatedFinalValue();
                isDirty = false;
            }
            return _value;
        } 
    }

    public PlayerBaseStats()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public PlayerBaseStats(float baseValue) : this() 
    {
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    public StatModifier GetModifierByItem(UsableItem item)
    {
        foreach (var modifier in statModifiers)
        {
            // Make sure 'item' is being used correctly here
            if (modifier.Source == item)
            {
                return modifier;
            }
        }
        return null;
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if(a.Order < b.Order)
            return -1;
        else if(a.Order > b.Order)
            return 1;
        return 0; // a == b
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if(statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for(int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if(statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    protected virtual float CalculatedFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for(int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            
            if(mod.Type == StatModType.Flat)
            {
                finalValue += mod.Value;
            }
            else if(mod.Type == StatModType.PercenAdd)
            {
                sumPercentAdd += mod.Value;

                if(i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercenAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if(mod.Type == StatModType.PercentMult)
            {
                finalValue *= 1 + mod.Value;
            }
            
        }
        return Mathf.RoundToInt(finalValue);
    }
}
