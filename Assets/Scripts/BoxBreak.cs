using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxBreak : MonoBehaviour
{
    [SerializeField] AudioSource breakingSFX;
    ParticleSystem particle;
    SpriteRenderer sr;

    bool isHit = false;

    void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(!isHit)
        {
            if(other.tag == "PlayerHitbox")
        {
            isHit = true;
            StartCoroutine(Break());
        }    
        }
    }

    IEnumerator Break()
    {
        particle.Play();
        breakingSFX.Play();
        sr.enabled = false; 
        GetComponent<LootBag>().InstantiateLoot(transform.position);

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject);
    }
}
