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
    public bool IsStunned { get; set; }


    //Controle
    protected bool _canTakeHit { get; set; }


    //Necessitam estanciar
    public Rigidbody2D Rig { get; set; }
    protected SpriteRenderer MySprite { get; set; }

    public virtual void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
        MySprite = GetComponent<SpriteRenderer>();
        IsStunned = false;
        InitialPosition = transform.position;
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

    void Die()
    {
        IsStunned = true;
        MySprite.color = new Color(0, 0, 0, 1);
        Destroy(gameObject, 2);
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

        yield return new WaitForSeconds(1);
        _canTakeHit = true;
    }
}
