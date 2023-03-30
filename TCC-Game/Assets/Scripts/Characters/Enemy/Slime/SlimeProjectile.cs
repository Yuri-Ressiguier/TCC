
using UnityEngine;


public class SlimeProjectile : Projectile
{
    private Player _player;
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
}
