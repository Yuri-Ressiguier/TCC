using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : Enemy
{
    //Delays
    [field: SerializeField] public float MoveAtkTimeDelay { get; set; }
    [field: SerializeField] public float ProjectileAtkTimeDelay { get; set; }


    //Ranges
    [field: SerializeField] public float MoveAtkRange { get; set; }
    [field: SerializeField] public float ProjectileAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }


    //Controles

    private bool _canMoveAtk { get; set; }
    private bool _canProjectileAtk { get; set; }
    private bool _canWalk { get; set; }



    //Outros
    public SlimeAnim AnimScript { get; set; }




    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<SlimeAnim>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _canMoveAtk = true;
        _canProjectileAtk = true;
        //_canWalk = true;
        _canTakeHit = true;
        _agent.enabled = true;
        Rig.isKinematic = true;

    }


    private void FixedUpdate()
    {
        if (!IsStunned)
        {
            CheckRanges();
        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Player" && collision.collider == _player.MainCollider)
        {
            _player.TakeHit(Power / 5);
        }

    }

    void CheckRanges()
    {

        Collider2D setDestination = Physics2D.OverlapCircle(transform.position, SetDestinationRange, PlayerLayer);
        Collider2D moveAtk = Physics2D.OverlapCircle(transform.position, MoveAtkRange, PlayerLayer);
        Collider2D projectileAtk = Physics2D.OverlapCircle(transform.position, ProjectileAtkRange, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (moveAtk != null && _canMoveAtk)
        {
            MoveAttack();
        }
        //else if (projectileAtk != null && _canProjectileAtk)
        //{
        //    _agent.isStopped = true;


        //}
        else if (setDestination != null && _agent.isActiveAndEnabled)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_player.transform.position);

        }
        else
        {
            //Lógica para perambular

        }



    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, SetDestinationRange);
        Gizmos.DrawWireSphere(transform.position, ProjectileAtkRange);
        Gizmos.DrawWireSphere(transform.position, MoveAtkRange);

    }

    void MoveAttack()
    {
        LastMoveDirection = _agent.velocity;
        DisableAgent();
        Rig.AddForce(LastMoveDirection * 40, ForceMode2D.Force);
        _canMoveAtk = false;
        StartCoroutine("MoveAtkDelay");
        AnimScript.MoveAttack(LastMoveDirection.x, LastMoveDirection.y);
    }

    void ProjectileAttack()
    {

    }



    //Coroutines

    IEnumerator MoveAtkDelay()
    {
        yield return new WaitForSeconds(MoveAtkTimeDelay);
        _canMoveAtk = true;
    }
    IEnumerator ProjectileAtkDelay()
    {
        yield return new WaitForSeconds(ProjectileAtkTimeDelay);
        _canProjectileAtk = true;
    }


}
