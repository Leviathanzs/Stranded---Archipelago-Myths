using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    Collider2D col;

    public List<Collider2D> detectedColliders = new List<Collider2D>();
    
    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        detectedColliders.Add(other);    
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        detectedColliders.Remove(other);
    }
}
