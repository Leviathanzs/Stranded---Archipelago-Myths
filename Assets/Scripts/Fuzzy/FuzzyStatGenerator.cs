using UnityEngine;

public class FuzzyStatGenerator : MonoBehaviour
{
    [Header("Input")]
    public int killCount = 10;
    public float completionTime = 80f;

    [System.Serializable]
    public struct Stats
    {
        public int strength;
        public int intelligence;
        public int agility;
        public int vitality;
    }

    void Start()
    {
        float attributeValue = GetAttributeValue(killCount, completionTime);
        string category = GetAttributeCategory(attributeValue);

        Debug.Log($"Kill: {killCount}, Time: {completionTime} sec");
        Debug.Log($"Attribute value: {attributeValue:F2} → Category: {category}");

        Stats stat = GenerateStatsFromCategory(category);
        Debug.Log($"[Generated Stats]");
        Debug.Log($"Strength     : {stat.strength}");
        Debug.Log($"Intelligence : {stat.intelligence}");
        Debug.Log($"Agility      : {stat.agility}");
        Debug.Log($"Vitality     : {stat.vitality}");
    }

    // Main Fuzzy Logic
    float GetAttributeValue(int kill, float time)
    {
        // Membership player_kills
        float pkLow = TrapMF(kill, 0, 0, 3, 7);
        float pkMed = TriMF(kill, 4, 7, 10);
        float pkHigh = TrapMF(kill, 7, 10, 15, 15);

        // Membership completion_time
        float ctFast = TrapMF(time, 0, 0, 30, 90);
        float ctMed = TriMF(time, 45, 90, 135);
        float ctSlow = TrapMF(time, 90, 150, 180, 180);

        // Apply fuzzy rules with adjusted weight priority
        float weak = Mathf.Max(
            Mathf.Min(pkLow, ctFast),
            Mathf.Min(pkLow, ctMed),
            Mathf.Min(pkLow, ctSlow),
            Mathf.Min(pkMed, ctSlow)
        );

        float average = Mathf.Max(
            Mathf.Min(pkHigh, ctSlow),
            Mathf.Min(pkMed, ctMed)
        );

        float strong = Mathf.Max(
            Mathf.Min(pkHigh, ctFast),
            Mathf.Min(pkHigh, ctMed),
            Mathf.Min(pkMed, ctFast) * 0.7f  // weakened medium+fast
        );

        float sum = weak + average + strong;

        // Defuzzification using weighted average
        return (sum == 0) ? 0.5f : (
            (weak * 0.25f + average * 0.5f + strong * 0.85f) / sum
        );
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

    // Membership functions
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
        if (x == b) return 1f;
        if (x > a && x < b) return (x - a) / (b - a);
        return (c - x) / (c - b);
    }
}
