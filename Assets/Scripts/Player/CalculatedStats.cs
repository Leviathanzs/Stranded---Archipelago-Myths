using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CalculatedStats : MonoBehaviour
{
    [SerializeField] Character stats;

    private int damage;
    private int HP;
    private float damageMultipler = 1f;
    private float hpMultipler = 50f;

    public int StrenghtToDamage {get { return CalculatedStrenghtToDamage();} }
    public int StrenghtToHP {get { return CalculatedStrenghtToHP();}}

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
}
