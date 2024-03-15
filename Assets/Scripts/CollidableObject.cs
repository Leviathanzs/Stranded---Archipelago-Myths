using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    private Collider2D col;
    [SerializeField] private ContactFilter2D filter;
    private List<Collider2D> collidedObject = new List<Collider2D>(1);

    protected virtual void Start()
    {
        col = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        col.OverlapCollider(filter, collidedObject);
        foreach(var o in collidedObject)
        {
            OnCollided(o.gameObject);
        }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        Debug.Log("Collided with " + collidedObject.name);
    }
}
