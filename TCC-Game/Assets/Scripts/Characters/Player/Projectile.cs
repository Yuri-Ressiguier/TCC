using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [field: SerializeField] public float Damage { get; set; }
    public Vector2 Direction { get; set; }
    public Rigidbody2D Rig { get; set; }

    void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 8);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy other = collision.gameObject.GetComponent<Enemy>();
            other.TakeHit(Damage);
        }
    }
}
