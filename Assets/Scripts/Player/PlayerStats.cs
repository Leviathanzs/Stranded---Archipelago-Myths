using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    Damageable damageable;
    [SerializeField]HealthBar healthBar;

    [SerializeField] int currentExperience, maxExperience, currentLevel;

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
        }
    }

    void LevelUp()
    {
        damageable.MaxHealth += 100;
        damageable.Health = damageable.MaxHealth;
        currentLevel++;
        currentExperience -= maxExperience; // Adjusting currentExperience after leveling up
        maxExperience += 100;

        // Ensure healthBar reference is not null before calling methods on it
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(damageable.MaxHealth);
            healthBar.LevelText(currentLevel.ToString());
        }
    }

    public void UpdateExp()
    {
        healthBar.SetMaxExp(maxExperience, currentExperience);
        healthBar.ExpPercentage(CalculateExperiencePercentage(currentExperience, maxExperience).ToString("N2") + "%");
    }
}
