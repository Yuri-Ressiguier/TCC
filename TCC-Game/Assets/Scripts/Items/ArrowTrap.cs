using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [field: SerializeField] private Projectile _projectilePrefab { get; set; }
    [field: SerializeField] private Vector2 _direction { get; set; }
    [field: SerializeField] private float _dmg { get; set; }
    [field: SerializeField] public float _appliedForce { get; set; }

    public void Shoot()
    {
        Projectile projectile = Instantiate(_projectilePrefab, this.transform.position, this.transform.rotation);
        projectile.GetComponent<Projectile>().Direction = _direction;
        projectile.GetComponent<Projectile>().Damage = _dmg;
        projectile.GetComponent<Rigidbody2D>().velocity = _direction * _appliedForce;
    }
}
