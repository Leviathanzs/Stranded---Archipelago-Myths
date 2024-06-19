using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : Attack
{
     [SerializeField] private Character character;
     private float damageMultiplier = 1.2f;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();

        Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? 
        Knocback : new Vector2(-Knocback.x, Knocback.y);
        
        if (damageable != null)
        {
            float damage = character.CalculateDamage() * damageMultiplier;
            damageable.Hit((int)damage, deliveredKnockback);
        }
    }
}
