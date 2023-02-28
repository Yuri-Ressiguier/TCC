using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int Value { get; set; }
    public Animator Anim { get; set; }
    public Rigidbody2D Rig { get; set; }

    void Start()
    {
        Anim = GetComponent<Animator>();
        Rig = GetComponent<Rigidbody2D>();
    }

    public void Pick()
    {
        Anim.SetTrigger("Pick");
    }

    //Chamado pelo anim
    public void DestroyCoin()
    {
        Destroy(gameObject, 0);
    }
}
