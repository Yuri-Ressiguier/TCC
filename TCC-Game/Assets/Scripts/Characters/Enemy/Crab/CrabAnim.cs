using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAnim : MonoBehaviour
{
    public Animator Anim { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public void Move(float directionX, int direction, float magnitude)
    {
        Anim.SetFloat("Horizontal", directionX);
        Anim.SetInteger("Direction", direction);
        Anim.SetFloat("Magnitude", magnitude);

    }

    public void MoveAttack(float directionX, int direction)
    {
        Anim.SetFloat("LastMoveHorizontal", directionX);
        Anim.SetInteger("Direction", direction);
        Anim.SetTrigger("MoveAtk");
    }
}
