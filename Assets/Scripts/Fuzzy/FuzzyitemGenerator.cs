using UnityEngine;
using System.Collections.Generic;

public class FuzzyItemStatGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct Stats
    {
        public int strength;
        public int intelligence;
        public int agility;
        public int vitality;

        public float strengthPercent;
        public float intelligencePercent;
        public float agilityPercent;
        public float vitalityPercent;
    }

    public Stats GenerateStats(int playerLevel, float rarity)
    {
        float attributeValue = GetAttributeValue(playerLevel, rarity);
        string category = GetAttributeCategory(attributeValue);

        Stats stat = GenerateStatsFromCategory(category);

        Debug.Log($"[Fuzzy Result] Level={playerLevel}, Rarity={rarity:F2}, Category={category}");
        return stat;
    }

    // Membership + Rule base + Defuzzification
    float GetAttributeValue(int level, float rarity)
    {
        // Membership Level
        float lvlLow = PlayerLevelLow(level);
        float lvlMed = PlayerLevelMedium(level);
        float lvlHigh = PlayerLevelHigh(level);

        // Membership Rarity
        float rarCommon = RarityCommon(rarity);
        float rarRare = RarityRare(rarity);
        float rarEpic = RarityEpic(rarity);

        // Rule base lengkap (9 kombinasi)
        List<(float antecedent, float consequent)> rules = new List<(float, float)>
        {
            (Mathf.Min(lvlLow, rarCommon), 0.25f),   // Low + Common → lemah
            (Mathf.Min(lvlLow, rarRare), 0.35f),    // Low + Rare → agak lemah
            (Mathf.Min(lvlLow, rarEpic), 0.45f),    // Low + Epic → menengah

            (Mathf.Min(lvlMed, rarCommon), 0.35f),  // Medium + Common → agak lemah
            (Mathf.Min(lvlMed, rarRare), 0.5f),     // Medium + Rare → sedang
            (Mathf.Min(lvlMed, rarEpic), 0.65f),    // Medium + Epic → agak kuat

            (Mathf.Min(lvlHigh, rarCommon), 0.45f), // High + Common → menengah
            (Mathf.Min(lvlHigh, rarRare), 0.65f),   // High + Rare → agak kuat
            (Mathf.Min(lvlHigh, rarEpic), 0.85f)    // High + Epic → kuat
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
        int minFlat = 1, maxFlat = 6;
        int minPercent = 1, maxPercent = 3; // weak → 1–3%

        if (category == "average")
        {
            minFlat = 7; maxFlat = 12;
            minPercent = 4; maxPercent = 7; // average → 4–7%
        }
        else if (category == "strong")
        {
            minFlat = 13; maxFlat = 20;
            minPercent = 8; maxPercent = 12; // strong → 8–12%
        }

        return new Stats
        {
            strength = Random.Range(minFlat, maxFlat + 1),
            intelligence = Random.Range(minFlat, maxFlat + 1),
            agility = Random.Range(minFlat, maxFlat + 1),
            vitality = Random.Range(minFlat, maxFlat + 1),

            strengthPercent = Random.Range(minPercent, maxPercent + 1) / 100f,
            intelligencePercent = Random.Range(minPercent, maxPercent + 1) / 100f,
            agilityPercent = Random.Range(minPercent, maxPercent + 1) / 100f,
            vitalityPercent = Random.Range(minPercent, maxPercent + 1) / 100f
        };
    }

    // Membership Functions untuk Level
    float PlayerLevelLow(float x)
    {
        if (x <= 10) return 1f;
        if (x > 10 && x < 20) return (20f - x) / 10f;
        return 0f;
    }

    float PlayerLevelMedium(float x)
    {
        if (x <= 10 || x >= 30) return 0f;
        if (x > 10 && x < 20) return (x - 10f) / 10f;
        if (x >= 20 && x <= 30) return (30f - x) / 10f;
        return 0f;
    }

    float PlayerLevelHigh(float x)
    {
        if (x < 20) return 0f;
        if (x >= 30 && x <= 40) return 1f;
        if (x >= 20 && x < 30) return (x - 20f) / 10f;
        return 0f;
    }

    // Membership Functions untuk Rarity
    float RarityCommon(float x)
    {
        if (x <= 1f) return 1f;
        if (x > 1f && x < 2f) return (2f - x) / 1f;
        return 0f;
    }

    float RarityRare(float x)
    {
        if (x <= 1f || x >= 3f) return 0f;
        if (x > 1f && x < 2f) return (x - 1f) / 1f;
        if (x >= 2f && x < 3f) return (3f - x) / 1f;
        return 0f;
    }

    float RarityEpic(float x)
    {
        if (x <= 2f) return 0f;
        if (x > 2f && x < 3f) return (x - 2f) / 1f;
        if (x >= 3f && x <= 4f) return 1f;
        return 0f;
    }
}
