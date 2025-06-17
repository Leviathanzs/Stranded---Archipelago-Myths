using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamage : Attack
{
    [SerializeField] private Character character;
    private float damageMultiplier = 1.2f;

    void Awake()
    {
        // Coba assign otomatis kalau belum diset
        if (character == null)
        {
            character = GetComponentInParent<Character>();
        }

        // Amankan untuk reassign saat scene load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Coba cari ulang karakter di scene baru
        if (character == null)
        {
            character = GetComponentInParent<Character>();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"PlayerDamage: Menyentuh {other.name}");
        Damageable damageable = other.GetComponent<Damageable>();

        Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ?
            Knocback : new Vector2(-Knocback.x, Knocback.y);

        if (damageable != null && character != null)
        {
            float damage = character.CalculateDamage() * damageMultiplier;
            damageable.Hit((int)damage, deliveredKnockback);
        }
        else if (character == null)
        {
            Debug.LogWarning("Character belum diset di PlayerDamage.");
        }
    }
}
