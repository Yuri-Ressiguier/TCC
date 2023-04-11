using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberTrigger : MonoBehaviour
{
    [field: SerializeField] private List<ArrowTrap> Traps { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("ENTROU");
            Traps.ForEach(x => x.Shoot());
        }
    }
}
