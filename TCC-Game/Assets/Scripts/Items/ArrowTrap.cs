using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [field: SerializeField] protected Arrow _projectilePrefab { get; set; }
    [field: SerializeField] protected Vector2 _direction { get; set; }
    [field: SerializeField] protected float _dmg { get; set; }
    [field: SerializeField] protected float _appliedForce { get; set; }
    [field: SerializeField] private AudioClip _sfx { get; set; }
    private ActorSFX _actorSFX { get; set; }

    private void Awake()
    {
        _actorSFX = GetComponent<ActorSFX>();
    }


    public void Shoot()
    {
        _actorSFX.PlaySFX(_sfx);
        Projectile projectile = Instantiate(_projectilePrefab, this.transform.position, this.transform.rotation);
        projectile.GetComponent<Projectile>().Direction = _direction;
        projectile.GetComponent<Projectile>().Damage = _dmg;
        projectile.GetComponent<Rigidbody2D>().velocity = _direction * _appliedForce;
        if (_direction.x < 0 || _direction.y < 0)
        {
            projectile.GetComponent<Arrow>().ChangeSprite();
        }
    }
}
