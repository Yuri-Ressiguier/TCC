using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [field: SerializeField] public Vector2 Direction { get; set; }
    [field: SerializeField] public GameObject ProjectilePrefab { get; set; }
    [field: SerializeField] public float AppliedForce { get; set; }

    public void Shoot(float dmg)
    {
        GameObject projectile = Instantiate(ProjectilePrefab, this.transform.position, this.transform.rotation);
        projectile.GetComponent<Projectile>().Direction = Direction;
        projectile.GetComponent<Projectile>().Damage = dmg;
        projectile.GetComponent<Rigidbody2D>().AddForce(Direction * AppliedForce, ForceMode2D.Force);

    }
}
