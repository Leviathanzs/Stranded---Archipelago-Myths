using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;

    [SerializeField] private DetectionZone attackZone;
    [SerializeField] private AudioSource hitAudioSource;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float walkStopRate = 0.05f;
    [SerializeField] int expAmount = 300;

    private bool isDead = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }
    private bool _hasTarget = false;

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
    }

    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        private set { animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        isDead = false;
        damageable.damageableDeath.AddListener(OnDeath);
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (!damageable.LockVelocity)
        {
            if (CanMove && IsAlive)
            {
                rb.velocity = new Vector2(walkSpeed, 0f);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            FlipEnemyFacing();
            walkSpeed = -walkSpeed;
        }
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Mathf.Sign(rb.velocity.x), 1f);
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnDeath()
    {
        if (isDead) return;

        isDead = true;
        ExperienceManager.Instance.AddExperience(expAmount);

        // Tambahkan animasi mati, efek partikel, atau destroy enemy di sini jika perlu
        // Destroy(gameObject); // contoh: musuh dihancurkan setelah mati
    }

    public void playHit()
    {
        hitAudioSource.Play();
    }
}
