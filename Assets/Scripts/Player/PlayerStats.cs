using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    Damageable damageable;
    [SerializeField]HealthBar healthBar;

    [SerializeField] int currentExperience, maxExperience, currentLevel;

    void Awake()
    {
        damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
    }

    private void OnDisable() 
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;    
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
        healthBar.SetMaxHealth(damageable.Health);


        currentLevel++;
        currentExperience = 0;
        maxExperience += 100;
    }
}
