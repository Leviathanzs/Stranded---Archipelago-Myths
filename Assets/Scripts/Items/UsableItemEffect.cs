using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItemEffect : ScriptableObject 
{
    public abstract void ExecuteEffect(UsableItem parentItem, Damageable character);   
    public abstract void ExecuteEffect2(UsableItem parentItem, Character character);   
    public abstract string GetDescription();
}
