using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int attackDamage = 10;
    [SerializeField] Vector2 knocback = Vector2.zero;


    void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();

        Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? 
        knocback : new Vector2(-knocback.x, knocback.y);
        if(damageable != null)
        {
            damageable.Hit(attackDamage, deliveredKnockback);
        }
    }

}