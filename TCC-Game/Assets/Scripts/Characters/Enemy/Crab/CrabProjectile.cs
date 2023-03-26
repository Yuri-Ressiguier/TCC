using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabProjectile : Projectile
{
    private Player _player;
    private Animator _anim;

    public void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public override void Start()
    {
        base.Start();
        _player = FindObjectOfType<Player>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.collider == _player.MainCollider)
        {
            Player other = collision.gameObject.GetComponent<Player>();
            other.TakeHit(Damage);
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void AnimDown()
    {
        Debug.Log(_anim);
        _anim.SetTrigger("Down");
    }

    public void AnimUp()
    {
        _anim.SetTrigger("Up");
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}
