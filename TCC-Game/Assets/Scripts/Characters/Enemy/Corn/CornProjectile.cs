using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornProjectile : Projectile
{
    private Player _player;
    [field: SerializeField] private Sprite VerticalImg { get; set; }
    public override void Start()
    {
        base.Start();
        _player = FindObjectOfType<Player>();
    }

    public void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = VerticalImg;
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
}
