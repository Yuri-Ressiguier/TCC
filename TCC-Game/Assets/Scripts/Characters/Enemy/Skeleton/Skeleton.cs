using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    //Delays
    [field: SerializeField] public float MaceAtkTimeDelay { get; set; }

    //Ranges
    [field: SerializeField] public float MaceAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }

    //Controles
    private bool _canMaceAtk { get; set; }

    //Outros
    public SkeletonAnim AnimScript { get; set; }

    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<SkeletonAnim>();
        StartCoroutine("MaceAtkDelay");
        //Rig.isKinematic = true;
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
        Collider2D maceAtk = Physics2D.OverlapCircle(transform.position, MaceAtkRange, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (maceAtk != null && _canMaceAtk && _agent.isActiveAndEnabled)
        {
            _canMaceAtk = false;
            ReturnToStartTimer = 0;
            MaceAttack();
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
        Gizmos.DrawWireSphere(transform.position, MaceAtkRange);

    }

    void MaceAttack()
    {
        LastMoveDirection = _agent.velocity;
        PauseAgent();

        StartCoroutine("MaceAtkDelay");
        AnimScript.MaceAttack(LastMoveDirection.x, LastMoveDirection.y);
    }

    //Coroutines

    IEnumerator MaceAtkDelay()
    {
        yield return new WaitForSeconds(MaceAtkTimeDelay);
        _canMaceAtk = true;
    }

}
