using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; 
    public UnityEvent damageableDeath;
    Animator animator;

    [SerializeField] int _maxHealth = 100;
    [SerializeField] int _health = 100;
    [SerializeField] int _maxMana = 100;
    [SerializeField] int _mana = 100;
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

    public int MaxMana{get {return _maxMana;} set {
        _maxMana = value;
    }}

    public int Mana{get {return _mana;} set {
        _mana = value;
    }}

    public bool IsAlive{get {return _isAlive;} private set {
        _isAlive = value;
        animator.SetBool(AnimationStrings.isAlive, value);

        if(value == false)
        {
            damageableDeath.Invoke();
        }
    }}

    public bool LockVelocity {get {return animator.GetBool(AnimationStrings.lockVelocity);} private set{
        animator.SetBool(AnimationStrings.lockVelocity, value);
    }}
    void Awake()
    {
        animator = GetComponent<Animator>();
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
