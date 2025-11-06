using UnityEngine;
using System.Collections.Generic;

public class FuzzyStatGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct Stats
    {
        public int strength;
        public int intelligence;
        public int agility;
        public int vitality;
    }

    public Stats GenerateStats(int killCount, float completionTime)
    {
        float attributeValue = GetAttributeValue(killCount, completionTime);
        string category = GetAttributeCategory(attributeValue);

        Stats stat = GenerateStatsFromCategory(category);

        Debug.Log($"[Fuzzy Result] Kill={killCount}, Time={completionTime:F2}, Category={category}");
        return stat; // ← penting, supaya bisa dikembalikan ke StageController
    }

    // Membership + Rule base + Defuzzification (pakai versi refactor sebelumnya)
    float GetAttributeValue(int kill, float time)
    {
        float pkLow = TrapMF(kill, 0, 0, 4, 7);
        float pkMed = TriMF(kill, 4, 7, 10);
        float pkHigh = TrapMF(kill, 7, 10, 14, 14);

        float ctFast = TrapMF(time, 0, 0, 45, 90);
        float ctMed = TriMF(time, 45, 90, 135);
        float ctSlow = TrapMF(time, 90, 135, 180, 180);

        List<(float antecedent, float consequent)> rules = new List<(float, float)>
        {
            (Mathf.Min(pkLow, ctFast), 0.25f),
            (Mathf.Min(pkLow, ctMed), 0.25f),
            (Mathf.Min(pkLow, ctSlow), 0.25f),
            (Mathf.Min(pkMed, ctSlow), 0.25f),
            (Mathf.Min(pkMed, ctMed), 0.5f),
            (Mathf.Min(pkHigh, ctSlow), 0.5f),
            (Mathf.Min(pkHigh, ctFast), 0.85f),
            (Mathf.Min(pkHigh, ctMed), 0.85f),
            (Mathf.Min(pkMed, ctFast), 0.7f)
        };

        float numerator = 0f, denominator = 0f;
        foreach (var rule in rules)
        {
            numerator += rule.antecedent * rule.consequent;
            denominator += rule.antecedent;
        }

        return (denominator == 0f) ? 0.5f : numerator / denominator;
    }

    string GetAttributeCategory(float value)
    {
        if (value < 0.4f) return "weak";
        else if (value < 0.7f) return "average";
        else return "strong";
    }

    Stats GenerateStatsFromCategory(string category)
    {
        int min = 1, max = 6;
        if (category == "average") { min = 7; max = 12; }
        else if (category == "strong") { min = 13; max = 20; }

        return new Stats
        {
            strength = Random.Range(min, max + 1),
            intelligence = Random.Range(min, max + 1),
            agility = Random.Range(min, max + 1),
            vitality = Random.Range(min, max + 1)
        };
    }

    float TrapMF(float x, float a, float b, float c, float d)
    {
        if (x <= a || x >= d) return 0f;
        if (x >= b && x <= c) return 1f;
        if (x > a && x < b) return (x - a) / (b - a);
        return (d - x) / (d - c);
    }

    float TriMF(float x, float a, float b, float c)
    {
        if (x <= a || x >= c) return 0f;
        if (Mathf.Approximately(x, b)) return 1f;
        if (x > a && x < b) return (x - a) / (b - a);
        return (c - x) / (c - b);
    }
}