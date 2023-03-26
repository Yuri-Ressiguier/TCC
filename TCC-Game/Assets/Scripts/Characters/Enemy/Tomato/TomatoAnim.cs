using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoAnim : MonoBehaviour
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

    public void Rolling(int direction)
    {
        Anim.SetInteger("Direction", direction);
        Anim.SetBool("Rolling", true);
    }

    public void RollingOff()
    {
        Anim.SetBool("Rolling", false);
    }

    public void StunOn()
    {
        Anim.SetBool("Stun", true);
    }

    public void StunOff()
    {
        Anim.SetBool("Stun", false);
    }

}
