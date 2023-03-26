using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{
    [field: SerializeField] private Enemy _enemy;
    private Player _player;
    void Start()
    {
        _player = FindObjectOfType<Player>();
        Physics2D.IgnoreCollision(_enemy.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.collider == _player.MainCollider)
        {
            Player other = collision.gameObject.GetComponent<Player>();
            other.TakeHit(_enemy.Power * 2);
        }
    }
}
