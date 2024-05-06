using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CalculatedStats : MonoBehaviour
{
    [SerializeField] Character stats;

    private int damage;
    private int HP;
    private int mana;
    private float damageMultipler = 1.5f;
    private float hpMultipler = 5f;
    private float manaMultipler = 2f;

    public int StrenghtToDamage {get { return CalculatedStrenghtToDamage();} }
    public int StrenghtToHP {get { return CalculatedStrenghtToHP();}}
    public int IntelligenceToMana {get { return CalculatedIntelligenceToMana();}}

    private int CalculatedStrenghtToDamage()
    {
        damage = Mathf.RoundToInt(stats.StrenghtFinalValue * damageMultipler);
        
        return damage;
    }

    private int CalculatedStrenghtToHP()
    {
        HP = Mathf.RoundToInt(stats.StrenghtFinalValue * hpMultipler);

        return HP;
    }

    private int CalculatedIntelligenceToMana()
    {
        mana = Mathf.RoundToInt(stats.IntelligenceFinalValue * manaMultipler);
        return mana;
    }
}
