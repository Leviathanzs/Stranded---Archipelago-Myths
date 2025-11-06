using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyes : MonoBehaviour
{
    [SerializeField] float flightSpeed = 2f;
    [SerializeField] float waypointReachedDistance = .1f;
    [SerializeField] List<Transform> waypoints;
    [SerializeField] DetectionZone biteDetectionZone;
    [SerializeField] int expAmount = 100;
    [SerializeField] AudioSource hitAudioSource;
    public Collider2D deathCollider;

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool _hasTarget = false;
    public bool HasTarget{get {return _hasTarget;} private set 
    {
        _hasTarget = value;
        animator.SetBool(AnimationStrings.hasTarget, value);
    }}

    public bool CanMove
    {
        get 
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown 
    {
        get 
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        } 
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));  
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void OnEnable() 
    {
        damageable.damageableDeath.AddListener(OnDeath); 
    }

    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;

        if(AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate() 
    {
        if(damageable.IsAlive)
        {
            if(CanMove)
            {
                flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }  
    }

    private void flight()
    {
        //fly to waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        //see if we need to switch waypoints
        if(distance <= waypointReachedDistance)
        {
            //switch to next waypoint
            waypointNum++;

            if(waypointNum >= waypoints.Count)
            {
                //loop back to original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if(transform.localScale.x > 0)
        {
            //facing the right
            if(rb.velocity.x < 0)
            {
                //flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else 
        {
            //facing the left
             if(rb.velocity.x > 0)
            {
                //flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }
    
    public void OnDeath()
    {
        //Dead flyier fall to the ground
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;

        StageController stageController = FindObjectOfType<StageController>();
        if(stageController != null)
        {
            stageController.OnEnemyKilled();
        }

        ExperienceManager.Instance.AddExperience(expAmount);
    }

    public void playHit()
    {
        hitAudioSource.Play();
    }
}
