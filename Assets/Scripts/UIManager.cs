using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private GameObject healthtextPrefab;
    [SerializeField] private Canvas gameCanvas;

    void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
    }

    void OnEnable() 
    {
        CharacterEvents.characterDamaged += CharacterTookDamage;    
        CharacterEvents.characterHealed += CharacterHealed;    
    }

    void OnDisable() 
    {
        CharacterEvents.characterDamaged -= CharacterTookDamage;    
        CharacterEvents.characterHealed -= CharacterHealed;
    }

   void CharacterTookDamage(GameObject character, int damageReceived)
   {
        Vector2 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, quaternion.identity, 
            gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
   }

   void CharacterHealed(GameObject character, int healthRestored)
   {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthtextPrefab, spawnPosition, quaternion.identity, 
            gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();
   }
}
