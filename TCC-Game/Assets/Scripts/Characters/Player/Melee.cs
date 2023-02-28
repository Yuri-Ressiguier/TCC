using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [field: SerializeField] Player _player;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Enemy other = collision.gameObject.GetComponent<Enemy>();

                if (other != null)
                {
                    other.TakeHit(_player.Power);
                }
            }
        }

    }
}
