using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public static int Value { get; set; }
    public Rigidbody2D Rig { get; set; }

    void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
        Value = 10;
    }

}
