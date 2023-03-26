using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math_01 : Enemy
{
    //Delays
    [field: SerializeField] public float MeleeAtkTimeDelay { get; set; }


    //Ranges
    [field: SerializeField] public float MeleeAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }


    //Controles
    private bool _canMeleeAtk { get; set; }

    //Outros
    public Math_01_Anim AnimScript { get; set; }


    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<Math_01_Anim>();
        StartCoroutine("MeleeAtkDelay");
        Rig.isKinematic = true;
    }


    private void FixedUpdate()
    {
        if (!IsStunned)
        {
            CheckRanges();
        }
    }

    void CheckRanges()
    {

        Collider2D setDestination = Physics2D.OverlapCircle(transform.position, SetDestinationRange, PlayerLayer);
        Collider2D meleeAtk = Physics2D.OverlapCircle(transform.position, MeleeAtkRange, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (meleeAtk != null && _canMeleeAtk && _agent.isActiveAndEnabled)
        {
            _canMeleeAtk = false;
            ReturnToStartTimer = 0;
            MeleeAttack();
        }
        else if (setDestination != null && _agent.isActiveAndEnabled)
        {
            ReturnToStartTimer = 0;
            _agent.SetDestination(_player.transform.position);

        }
        else
        {
            //Perambular

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, SetDestinationRange);
        Gizmos.DrawWireSphere(transform.position, MeleeAtkRange);

    }

    void MeleeAttack()
    {
        LastMoveDirection = _agent.velocity;
        PauseAgent();
        StartCoroutine("MeleeAtkDelay");
        AnimScript.MeleeAttack(LastMoveDirection.x, LastMoveDirection.y);
    }

    //Coroutines

    IEnumerator MeleeAtkDelay()
    {
        yield return new WaitForSeconds(MeleeAtkTimeDelay);
        _canMeleeAtk = true;
    }

}
