using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxBreak : MonoBehaviour
{
    ParticleSystem particle;
    SpriteRenderer sr;

    void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "PlayerHitbox")
        {
            StartCoroutine(Break());
        }    
    }

    IEnumerator Break()
    {
        particle.Play();
        sr.enabled = false;

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject);
    }
}
