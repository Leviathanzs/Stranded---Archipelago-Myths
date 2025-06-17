using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    // Komponen internal
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDirections touchingDirections;
    private Damageable damageable;
    private new Transform transform;

    // Komponen yang bisa diset di Inspector atau dicari ulang
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private InventoryInput inventoryInput;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioSource attackAudioSource;
    [SerializeField] private AudioSource hitAudioSource;

    // Movement dan state
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float airWalkSpeed = 3f;
    [SerializeField] private float jumpImpulse = 10f;
    [SerializeField] private float attackImpulse = 0.2f;

    private float footstepTimer = 0f;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] private bool _isFacingRight = true;

    // Properti eksternal
    public float CurrentMoveSpeed => (CanMove && IsAlive && IsMoving && !touchingDirections.IsOnWall)
        ? (touchingDirections.IsGrounded ? (_isRunning ? runSpeed : walkSpeed) : airWalkSpeed)
        : 0f;

    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            animator?.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            animator?.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
                transform.localScale *= new Vector2(-1, 1);

            _isFacingRight = value;
        }
    }

    public bool CanMove => animator != null && animator.GetBool(AnimationStrings.canMove);
    public bool IsAlive => animator != null && animator.GetBool(AnimationStrings.isAlive);

    // === Unity Lifecycle ===
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        transform = GetComponent<Transform>();

        InitializeComponents();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void FixedUpdate()
    {
        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator?.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        if (IsMoving && touchingDirections.IsGrounded)
        {
            float speedFactor = CurrentMoveSpeed / runSpeed;
            float interval = Mathf.Lerp(0.5f, 0.2f, speedFactor);

            footstepTimer -= Time.fixedDeltaTime;
            if (footstepTimer <= 0f)
            {
                footstepAudioSource?.Play();
                footstepTimer = interval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    // === Scene Load Handler ===
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        if (inventoryInput == null)
            inventoryInput = FindObjectOfType<InventoryInput>();

        if (healthBar == null)
            healthBar = FindObjectOfType<HealthBar>();

        if (footstepAudioSource == null)
            footstepAudioSource = GetComponent<AudioSource>();

        if (attackAudioSource == null)
            attackAudioSource = GetComponent<AudioSource>();

        if (hitAudioSource == null)
            hitAudioSource = GetComponent<AudioSource>();
    }

    // === Input Methods ===
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        if (IsAlive)
            SetFacingDirection(moveInput);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator?.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && inventoryInput != null && !inventoryInput.IsOpen)
        {
            animator?.SetTrigger(AnimationStrings.attackTrigger);

            float impulse = IsFacingRight ? attackImpulse : -attackImpulse;
            transform.position = new Vector2(transform.position.x + impulse, transform.position.y);
        }
    }

    public void ShowMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // === Damage Handler ===
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        healthBar?.SetHealth(damageable.Health);

         if (!IsAlive && deathUI != null)
        {
            deathUI.SetActive(true);
            ShowMouseCursor();
        }
    }

    // === Audio Triggers ===
    public void playAttack() => attackAudioSource?.Play();
    public void playHit() => hitAudioSource?.Play();

    // === Utility ===
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
            IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight)
            IsFacingRight = false;
    }

    public void ResetPlayer()
    {
        // Reset position ke spawn point
        GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
        }

        // Reset health/mana
        if (damageable != null)
        {
            damageable.Health = damageable.MaxHealth;
            damageable.Mana = damageable.MaxMana;
        }

        if (healthBar != null && damageable != null)
        {
            healthBar.SetMaxHealth(damageable.MaxHealth, damageable.Health);
        }

        // Reset animation
        animator.SetBool(AnimationStrings.isAlive, true);
        animator.SetBool(AnimationStrings.canMove, true);
        animator.SetBool(AnimationStrings.isRunning, false);
        animator.SetBool(AnimationStrings.isMoving, false);
        animator.Rebind();
        animator.Update(0f);

        // Reset physics
        rb.velocity = Vector2.zero;

        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.ClearInventory();
        }
        
        // Matikan UI kematian
        if (deathUI != null)
            deathUI.SetActive(false);

        // Sembunyikan mouse kembali
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
