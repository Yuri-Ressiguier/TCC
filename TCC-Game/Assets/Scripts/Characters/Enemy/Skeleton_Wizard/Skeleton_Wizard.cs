using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Wizard : Enemy
{
    //Delays
    [field: SerializeField] public float PoisonAtkTimeDelay { get; set; }
    [field: SerializeField] public float AreaAtkTimeDelay { get; set; }


    //Ranges
    [field: SerializeField] public float PoisonAtkRange { get; set; }
    [field: SerializeField] public float AreaAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }


    //Controles
    private bool _canPoisonAtk { get; set; }
    private bool _canAreaAtk { get; set; }
    private bool _isPoisonAtk { get; set; }
    private bool _isAreaAtk { get; set; }

    //Outros
    public Skeleton_Wizard_Anim AnimScript { get; set; }


    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<Skeleton_Wizard_Anim>();
        StartCoroutine("PoisonAtkDelay");
        StartCoroutine("AreaAtkDelay");
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
        Collider2D poisonAtk = Physics2D.OverlapCircle(transform.position, PoisonAtkRange, PlayerLayer);
        Collider2D areaAtk = Physics2D.OverlapCircle(transform.position, AreaAtkRange, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (poisonAtk != null && _canPoisonAtk && _agent.isActiveAndEnabled && !_isAreaAtk)
        {
            _canPoisonAtk = false;
            ReturnToStartTimer = 0;
            PoisonAttack();
        }
        else if (areaAtk != null && _canAreaAtk && _agent.isActiveAndEnabled && !_isPoisonAtk)
        {
            _canAreaAtk = false;
            ReturnToStartTimer = 0;
            StartCoroutine("AreaAttack");
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
        Gizmos.DrawWireSphere(transform.position, AreaAtkRange);
        Gizmos.DrawWireSphere(transform.position, PoisonAtkRange);

    }

    void PoisonAttack()
    {
        LastMoveDirection = _agent.velocity;
        _isPoisonAtk = true;
        PauseAgent();
        AnimScript.PoisonAttack(LastMoveDirection.x, LastMoveDirection.y);
    }

    void PoisonAttackOff()
    {
        UnpauseAgent();
        StartCoroutine("PoisonAtkDelay");
        _isPoisonAtk = false;
    }

    void AreaAttack()
    {
        LastMoveDirection = _agent.velocity;
        _isAreaAtk = true;
        PauseAgent();
        AnimScript.AreaAttack(LastMoveDirection.x, LastMoveDirection.y);
    }

    void AreaAttackOff()
    {
        UnpauseAgent();
        StartCoroutine("AreaAtkDelay");
        _isAreaAtk = false;
    }

    //Coroutines

    IEnumerator PoisonAtkDelay()
    {
        yield return new WaitForSeconds(PoisonAtkTimeDelay);
        _canPoisonAtk = true;
    }
    IEnumerator AreaAtkDelay()
    {
        yield return new WaitForSeconds(AreaAtkTimeDelay);
        _canAreaAtk = true;
    }
}
