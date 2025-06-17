using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Damageable damageable;
    [SerializeField] private StatPanel displayStatValues;
    [SerializeField] private Character statValues;
    private HealthBar healthBar;
    private TextMeshProUGUI levelTextPanel;

    [SerializeField] private int currentExperience, maxExperience, currentLevel;
    private bool _isLevelUp = false;

    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public bool IsLevelUp { get => _isLevelUp; set => _isLevelUp = value; }

    void Awake()
    {
        InitializeSceneComponents(); // Inisialisasi awal
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (ExperienceManager.Instance != null)
            ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
        else
            Debug.LogWarning("ExperienceManager instance is null!");

        TryAssignStatValues();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeSceneComponents();
        TryAssignStatValues();
    }

    private void TryAssignStatValues()
    {
        if (statValues == null)
            statValues = FindObjectOfType<Character>();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (ExperienceManager.Instance != null)
            ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
    }

    private void InitializeSceneComponents()
    {
        if (damageable == null)
            damageable = GetComponent<Damageable>();

        if (healthBar == null)
            healthBar = FindObjectOfType<HealthBar>();

        if (displayStatValues == null)
            displayStatValues = FindObjectOfType<StatPanel>();

        if (statValues == null)
            statValues = GetComponent<Character>();

        if (levelTextPanel == null)
        {
            GameObject levelObj = GameObject.Find("Level_Text");
            if (levelObj != null)
                levelTextPanel = levelObj.GetComponent<TextMeshProUGUI>();
            else
                Debug.LogWarning("UI element 'Level_Text' not found in scene.");
        }
    }

    public void UpdateExp()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxExp(maxExperience, currentExperience);
            float percentage = CalculateExperiencePercentage(currentExperience, maxExperience);
            healthBar.ExpPercentage(percentage.ToString("N2") + "%");
        }
    }

    private float CalculateExperiencePercentage(int currentExp, int maxExp)
    {
        if (maxExp <= 0)
        {
            Debug.LogError("Max experience points should be greater than zero.");
            return 0f;
        }

        float percentage = (float)currentExp / maxExp * 100f;
        return Mathf.Clamp(percentage, 0f, 100f);
    }

    private void HandleExperienceChange(int newExperience)
    {
        currentExperience += newExperience;
        if (currentExperience >= maxExperience)
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
        currentExperience -= maxExperience;
        maxExperience += 100;

        statValues.RecalculateBaseStats();
        displayStatValues?.UpdateStatValues();

        healthBar?.SetMaxHealth(damageable.MaxHealth, damageable.Health);
        healthBar?.LevelText(currentLevel.ToString());

        if (levelTextPanel != null)
            levelTextPanel.text = currentLevel.ToString();

        statValues.originalMaxHealth = damageable.MaxHealth;
        statValues.originalMaxMana = damageable.MaxMana;
    }

    public void HpLevelUp()
    {
        int strengthHP = (int)(statValues.Strenght.BaseValue * 2);
        int vitalityHP = (int)(statValues.Vitality.BaseValue * 5);
        damageable.MaxHealth += strengthHP + vitalityHP;
    }

    public void ManaLevelUp()
    {
        int intelligenceMana = (int)(statValues.Intelligence.BaseValue * 3);
        damageable.MaxMana += intelligenceMana;
    }
}
