using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //Status
    [field: SerializeField] public float Life { get; set; }
    [field: SerializeField] public float Power { get; set; }
    public Vector2 LastMoveDirection { get; set; }
    public Vector2 InitialPosition { get; set; }
    public float LifeCap { get; set; }
    public bool IsStunned { get; set; }


    //Controle
    protected bool _canTakeHit { get; set; }


    //Necessitam estanciar
    public Rigidbody2D Rig { get; set; }
    protected SpriteRenderer MySprite { get; set; }
    protected Collider2D Col { get; set; }

    public virtual void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
        MySprite = GetComponent<SpriteRenderer>();
        Col = GetComponent<Collider2D>();
        IsStunned = false;
        InitialPosition = transform.position;
        LifeCap = Life;
        _canTakeHit = true;
    }


    public virtual void TakeHit(float dmg)
    {
        if (_canTakeHit)
        {
            Life = (float)Math.Round(Life - dmg);
            if (Life <= 0)
            {
                Die();
            }
            else
            {
                _canTakeHit = false;
                StartCoroutine("TakeHitDelay");
            }
        }
    }

    public virtual void Die()
    {
        IsStunned = true;
        MySprite.enabled = false;
        Col.enabled = false;
        Destroy(gameObject, 1.5f);
    }

    IEnumerator TakeHitDelay()
    {
        MySprite.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.2f);
        MySprite.color = new Color(1, 1, 1, 1);

        for (int i = 0; i < 7; i++)
        {
            MySprite.enabled = false;
            yield return new WaitForSeconds(0.15f);
            MySprite.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
        _canTakeHit = true;
    }
}
