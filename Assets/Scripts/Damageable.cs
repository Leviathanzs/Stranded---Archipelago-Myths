using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; 
    Animator animator;
    CalculatedStats stats;
    [SerializeField] Character calculatedStats;

    [SerializeField] int _maxHealth = 100;
    [SerializeField] int _health = 100;
    bool _isAlive = true;

    public int MaxHealth{get {return _maxHealth;} set {
        _maxHealth = value;
    }}

    public int Health{get {return _health;} set {
        _health = value;

        if(_health <= 0)
        {
            IsAlive = false;
        }
    }}

    public bool IsAlive{get {return _isAlive;} private set {
        _isAlive = value;
        animator.SetBool(AnimationStrings.isAlive, value);
    }}

    public bool LockVelocity {get {return animator.GetBool(AnimationStrings.lockVelocity);} private set{
        animator.SetBool(AnimationStrings.lockVelocity, value);
    }}
    void Awake()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<CalculatedStats>();    
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if(IsAlive)
        {
            Health -= damage;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            return true;
        }
        return false;
    }
}
