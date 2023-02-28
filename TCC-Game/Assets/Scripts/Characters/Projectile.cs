using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float Damage { get; set; }
    public Vector2 Direction { get; set; }
    public Rigidbody2D Rig { get; set; }

    public virtual void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
    }


}
