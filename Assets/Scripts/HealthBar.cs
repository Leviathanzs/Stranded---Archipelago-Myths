using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider manaBar;
    [SerializeField] TextMeshProUGUI levelText;

    public void SetMaxHealth(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
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
}
