using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider sliderBar;


    void Awake()
    {
        sliderBar = GetComponent<Slider>();
    }

    void SetMaxHealth(int maxHealth)
    {
        sliderBar.maxValue = maxHealth;
        sliderBar.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        sliderBar.value = health;
    }
}
