using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator Anim { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }


    public void Move(float directionX, float directionY, float magnitude, float lastMoveX, float lastMoveY)
    {
        Anim.SetFloat("Horizontal", directionX);
        Anim.SetFloat("Vertical", directionY);
        Anim.SetFloat("Magnitude", magnitude);

        Anim.SetFloat("LastMoveHorizontal", lastMoveX);
        Anim.SetFloat("LastMoveVertical", lastMoveY);
    }


    public void Attack()
    {
        Anim.SetTrigger("Attack");
    }

    public void Defense()
    {
        Anim.SetTrigger("Defense");
    }

    public void Interrupt()
    {
        Anim.SetTrigger("Interrupt");
    }

    public void TurnRangedModeOn()
    {
        Anim.SetBool("RangedMode", true);
    }

    public void TurnRangedModeOff()
    {
        Anim.SetBool("RangedMode", false);
    }

    public void MoveRanged(int orientation, float directionX, float directionY, float magnitude)
    {
        Anim.SetInteger("RangedModeOrientation", orientation);
        Anim.SetFloat("Horizontal", Math.Abs(directionX));
        Anim.SetFloat("Vertical", Math.Abs(directionY));
        Anim.SetFloat("Magnitude", magnitude);
    }


}
