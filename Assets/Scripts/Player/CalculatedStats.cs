using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CalculatedStats : MonoBehaviour
{
    [SerializeField] Character stats;

    private int damage;
    private float damageMultipler = 1f;

    public int StrenghtToDamage {get { return CalculatedStrenght();} }

    public int CalculatedStrenght()
    {
        damage = Mathf.RoundToInt(stats.StrenghtFinalValue * damageMultipler);
        
        return damage;
    }
}
