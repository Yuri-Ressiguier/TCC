using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapON : ArrowTrap
{
    [field: SerializeField] private float ArrowTimeDelay { get; set; }

    void Start()
    {
        StartCoroutine("ArrowAttackDelay");
    }

    IEnumerator ArrowAttackDelay()
    {
        yield return new WaitForSeconds(ArrowTimeDelay);
        Shoot();
        StartCoroutine("ArrowAttackDelay");
    }

}
