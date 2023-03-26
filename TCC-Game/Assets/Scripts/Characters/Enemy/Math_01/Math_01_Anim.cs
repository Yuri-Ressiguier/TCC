using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math_01_Anim : MonoBehaviour
{
    public Animator Anim { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public void Move(float directionX, float directionY, float magnitude)
    {
        Anim.SetFloat("Horizontal", directionX);
        Anim.SetFloat("Vertical", directionY);
        Anim.SetFloat("Magnitude", magnitude);

    }

    public void MeleeAttack(float directionX, float directionY)
    {
        Anim.SetFloat("LastMoveHorizontal", directionX);
        Anim.SetFloat("LastMoveVertical", directionY);
        Anim.SetTrigger("MeleeAtk");
    }

}
