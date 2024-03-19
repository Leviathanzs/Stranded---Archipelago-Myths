using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : CollidableObject
{
    Animator animator;

    private bool isInteracted = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        if(Input.GetKey(KeyCode.F))
        {
            OnInteract();
        }
    }

    void OnInteract()
    {
        if(!isInteracted)
        {
            isInteracted = true;
            animator.SetTrigger(AnimationStrings.openTrigger);
            GetComponent<LootChest>().InstantiateLoot(transform.position);
        }
    }
}
