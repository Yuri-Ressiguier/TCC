using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [field: SerializeField] private float Damage { get; set; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("detectou");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("ENTROU");
            Player other = collision.gameObject.GetComponent<Player>();
            other.TakeHit(Damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("detectou");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("ENTROU");
            Player other = collision.gameObject.GetComponent<Player>();
            other.TakeHit(Damage);
        }
    }
}
