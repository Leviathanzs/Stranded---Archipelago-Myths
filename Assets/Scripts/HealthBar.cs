using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider manaBar;
    [SerializeField] Slider expBar;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI expPercentage;

    public static HealthBar healthInstance;

    public void Awake()
    {
        // Singleton pattern initialization
        if (healthInstance == null)
        {
            healthInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMaxHealth(int maxHealth, int health)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
    }

    public void SetMaxMana(int maxMana)
    {

    }

    public void SetMana(int mana)
    {

    }

    public void LevelText(string level)
    {
        levelText.text = level;
    }

    public void SetMaxExp(int maxExp, int currentExp)
    {
        expBar.maxValue = maxExp;
        expBar.value = currentExp;
    }

    public void SetExp(int exp)
    {
        expBar.value = exp;
    }

    public void ExpPercentage(string exp)
    {
        expPercentage.text = exp;
    }
}
