using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    Damageable damageable;
    [SerializeField] StatPanel displayStatValues;
    [SerializeField] Character statValues;
    [SerializeField] HealthBar healthBar;
    [SerializeField] TextMeshProUGUI levelTextPanel;
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
        damageable = GetComponent<Damageable>();
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
        damageable.MaxHealth += 100;
        damageable.Health = damageable.MaxHealth;
        currentLevel++;
        currentExperience -= maxExperience; // Adjusting currentExperience after leveling up
        maxExperience += 100;

        statValues.Strenght.BaseValue += 5;
        statValues.Agility.BaseValue += 5;
        statValues.Intelligence.BaseValue += 5;
        statValues.Vitality.BaseValue += 5;

        // Ensure healthBar reference is not null before calling methods on it
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(damageable.MaxHealth);
            healthBar.LevelText(currentLevel.ToString());
        }

        displayStatValues.UpdateStatValues();
        statValues.RecalculateBaseStats();

        levelTextPanel.text = currentLevel.ToString();
    }

    public void UpdateExp()
    {
        healthBar.SetMaxExp(maxExperience, currentExperience);
        healthBar.ExpPercentage(CalculateExperiencePercentage(currentExperience, maxExperience).ToString("N2") + "%");
    }
}
