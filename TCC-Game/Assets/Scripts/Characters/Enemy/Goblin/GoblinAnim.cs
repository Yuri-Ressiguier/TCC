using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnim : MonoBehaviour
{
    public Animator Anim { get; set; }

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

    public void ScavengerAttack(float directionX, float directionY)
    {
        Anim.SetFloat("LastMoveHorizontal", directionX);
        Anim.SetFloat("LastMoveVertical", directionY);
        Anim.SetBool("ScavengerAtk", true);
    }

    public void ScavengerAttackOff()
    {
        Anim.SetBool("ScavengerAtk", false);
    }

    public void WhirlwindAttackOn()
    {
        Anim.SetBool("WhirlwindAtk", true);
    }

    public void WhirlwindAttackOff()
    {
        Anim.SetBool("WhirlwindAtk", false);
    }
}
