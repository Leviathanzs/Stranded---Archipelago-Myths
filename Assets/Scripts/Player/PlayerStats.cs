using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]Damageable damageable;
    [SerializeField]StatPanel displayStatValues;
    [SerializeField]Character statValues;
    HealthBar healthBar;
    TextMeshProUGUI levelTextPanel;
    [SerializeField] int currentExperience, maxExperience, currentLevel;

    public int CurrentLevel {get { return currentLevel; } set { currentLevel = value; }}

    private bool _isLevelUp = false;
    public bool IsLevelUp {get { return _isLevelUp; } set { _isLevelUp = value; }}

    float CalculateExperiencePercentage(int currentExp, int maxExp)
    {
        if (maxExp <= 0)
        {
            Debug.LogError("Max experience points should be greater than zero.");
            return 0f; 
        }

        float percentage = (float)currentExp / maxExp * 100f;

        percentage = Mathf.Clamp(percentage, 0f, 100f);

        return percentage;
    }

    void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        levelTextPanel = GameObject.Find("Level_Text").GetComponent<TextMeshProUGUI>();
        if (healthBar == null)
        {
            Debug.LogError("HealthBar reference not assigned in PlayerStats!");
        }
    }

    private void OnEnable()
    {
        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
        }
        else
        {
            Debug.LogWarning("ExperienceManager instance is null!");
        }
    }

    private void OnDisable() 
    {
        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
        }
        else
        {
            Debug.LogWarning("ExperienceManager instance is null!");
        }
    }

    private void HandleExperienceChange(int newExperience)
    {
        currentExperience += newExperience;
        if(currentExperience >= maxExperience)
        {
            LevelUp();
            IsLevelUp = false;
        }
    }

    public void LevelUp()
    {
        IsLevelUp = true;

        statValues.Strenght.BaseValue += 5;
        statValues.Agility.BaseValue += 5;
        statValues.Intelligence.BaseValue += 5;
        statValues.Vitality.BaseValue += 5;

        HpLevelUp();
        ManaLevelUp();
        damageable.Health = damageable.MaxHealth;
        damageable.Mana = damageable.MaxMana;
        currentLevel++;
        currentExperience -= maxExperience; // Adjusting currentExperience after leveling up
        maxExperience += 100;

        displayStatValues.UpdateStatValues();
        statValues.RecalculateBaseStats();

        // Ensure healthBar reference is not null before calling methods on it
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(damageable.MaxHealth, damageable.Health);
            healthBar.LevelText(currentLevel.ToString());
        }

        levelTextPanel.text = currentLevel.ToString();
        statValues.originalMaxHealth = damageable.MaxHealth;
        statValues.originalMaxMana = damageable.MaxMana;
    }

    public void UpdateExp()
    {
        healthBar.SetMaxExp(maxExperience, currentExperience);
        healthBar.ExpPercentage(CalculateExperiencePercentage(currentExperience, maxExperience).ToString("N2") + "%");
    }

    public void HpLevelUp()
    {
        int strenghtHP = (int)statValues.Strenght.BaseValue * 2;
        int vitalityHP = (int)statValues.Vitality.BaseValue * 5;

        damageable.MaxHealth += strenghtHP + vitalityHP; 
    }

    public void ManaLevelUp()
    {
        int intelligenceMana = (int)statValues.Intelligence.BaseValue * 3;

        damageable.MaxMana += intelligenceMana;
    } 
}
