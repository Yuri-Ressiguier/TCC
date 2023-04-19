using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : Enemy
{
    //Delays
    [field: SerializeField] public float ScavengerAtkTimeDelay { get; set; }
    [field: SerializeField] public float WhirlwindAtkTimeDelay { get; set; }


    //Ranges
    [field: SerializeField] public float ScavengerAtkRange { get; set; }
    [field: SerializeField] public float WhirlwindAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }

    //Controles
    private bool _canScavengerAtk { get; set; }
    private bool _canWhirlwindAtk { get; set; }
    private bool _isWhirlwind { get; set; }

    private bool _isScavenger { get; set; }

    //Outros
    public GoblinAnim AnimScript { get; set; }

    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<GoblinAnim>();
        StartCoroutine("ScavengerAtkDelay");
        StartCoroutine("WhirlwindAtkDelay");
        Rig.isKinematic = true;
    }

    public override void Update()
    {
        base.Update();
        if (_isWhirlwind)
        {
            _agent.speed *= 1.5f;
        }
        else
        {
            _agent.speed = 0.8f;
        }
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
        Collider2D scavengerAtk = Physics2D.OverlapCircle(transform.position, ScavengerAtkRange, PlayerLayer);
        Collider2D projectileAtk = Physics2D.OverlapCircle(transform.position, WhirlwindAtkRange, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (scavengerAtk != null && _canScavengerAtk && _agent.isActiveAndEnabled && !_isWhirlwind)
        {
            _canScavengerAtk = false;
            ReturnToStartTimer = 0;
            ScavengerAttack();
        }
        else if (projectileAtk != null && _canWhirlwindAtk && _agent.isActiveAndEnabled && !_isScavenger)
        {
            _canWhirlwindAtk = false;
            ReturnToStartTimer = 0;
            WhirlwindAttack();
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
        Gizmos.DrawWireSphere(transform.position, WhirlwindAtkRange);
        Gizmos.DrawWireSphere(transform.position, ScavengerAtkRange);

    }

    void ScavengerAttack()
    {
        LastMoveDirection = _agent.velocity;
        PauseAgent();
        _isScavenger = true;
        AnimScript.ScavengerAttack(LastMoveDirection.x, LastMoveDirection.y);
    }

    //chamado no anim
    void ScavengerAttackOff()
    {
        _isScavenger = false;
        AnimScript.ScavengerAttackOff();
        UnpauseAgent();
        StartCoroutine("ScavengerAtkDelay");

    }

    void WhirlwindAttack()
    {
        PauseAgent();
        _isWhirlwind = true;
        AnimScript.WhirlwindAttackOn();
        UnpauseAgent();
        _agent.SetDestination(_player.transform.position);

    }

    //chamado no anim
    void WhirlwindAttackOff()
    {
        _isWhirlwind = false;
        StartCoroutine("WhirlwindAtkDelay");
        AnimScript.WhirlwindAttackOff();

    }

    //Coroutines

    IEnumerator ScavengerAtkDelay()
    {
        yield return new WaitForSeconds(ScavengerAtkTimeDelay);
        _canScavengerAtk = true;
    }
    IEnumerator WhirlwindAtkDelay()
    {
        yield return new WaitForSeconds(WhirlwindAtkTimeDelay);
        _canWhirlwindAtk = true;
    }


}
