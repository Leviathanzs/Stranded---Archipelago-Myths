using UnityEngine;

public class FuzzyStats : MonoBehaviour
{
    public int strength;
    public int intelligence;
    public int agility;
    public int vitality;

    // Fungsi untuk overwrite stat dari hasil fuzzy
    public void ApplyStats(FuzzyStatGenerator.Stats newStats)
    {
        strength = newStats.strength;
        intelligence = newStats.intelligence;
        agility = newStats.agility;
        vitality = newStats.vitality;

        Debug.Log($"[Fuzzy Stats Applied]");
        Debug.Log($"Strength     : {strength}");
        Debug.Log($"Intelligence : {intelligence}");
        Debug.Log($"Agility      : {agility}");
        Debug.Log($"Vitality     : {vitality}");
    }
}