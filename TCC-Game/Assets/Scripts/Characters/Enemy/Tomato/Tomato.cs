using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tomato : Enemy
{
    //Delays
    [field: SerializeField] public float RollingAtkTimeDelay { get; set; }

    //Ranges
    [field: SerializeField] public float RollingAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }


    //Controles
    private bool _canRollingAtk { get; set; }
    private Vector3 _targetPosition { get; set; }
    private bool _isRolling { get; set; }
    private float _tomatoInitialPower { get; set; }

    //SFX
    [field: SerializeField] private AudioClip _sfx { get; set; }
    private ActorSFX _actorSFX { get; set; }

    //Outros
    public TomatoAnim AnimScript { get; set; }
    [field: SerializeField] private int _meleeForce { get; set; }



    public override void Start()
    {
        base.Start();
        _actorSFX = GetComponent<ActorSFX>();
        AnimScript = GetComponent<TomatoAnim>();
        StartCoroutine("RollingAtkDelay");
        Rig.isKinematic = true;
        _tomatoInitialPower = Power;
    }

    public override void Update()
    {
        base.Update();
        if (_isRolling)
        {
            Power += 0.01f;
        }
        else
        {
            Power = _tomatoInitialPower;
        }
    }

    private void FixedUpdate()
    {
        if (!IsStunned && !_isRolling)
        {
            CheckRanges();
        }
    }

    void CheckRanges()
    {

        Collider2D setDestination = Physics2D.OverlapCircle(transform.position, SetDestinationRange, PlayerLayer);
        Collider2D moveAtk = Physics2D.OverlapCircle(transform.position, RollingAtkRange, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (moveAtk != null && _canRollingAtk && _agent.isActiveAndEnabled)
        {
            _canRollingAtk = false;
            ReturnToStartTimer = 0;
            StartCoroutine("RollingAttack");
        }
        else if (setDestination != null && _agent.isActiveAndEnabled && !_isRolling)
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
        Gizmos.DrawWireSphere(transform.position, RollingAtkRange);

    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (_isRolling)
        {
            AnimScript.StunOn();
            IsStunned = true;
            _isRolling = false;
            AnimScript.RollingOff();
            StartCoroutine("StunDelay");
        }

    }


    IEnumerator RollingAttack()
    {
        int direction;
        NavMeshHit hit;
        if (!_agent.Raycast(_player.transform.position, out hit))
        {
            _actorSFX.PlaySFX(_sfx);
            _targetPosition = hit.position;
            _agent.velocity = Vector3.zero;
            DisableAgent();
            Vector2 vec = (_targetPosition - gameObject.transform.position).normalized;

            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                direction = vec.x > 0 ? 1 : 3;
            }
            else
            {
                direction = vec.y > 0 ? 0 : 2;
            }
            _isRolling = true;
            AnimScript.Rolling(direction);

            yield return new WaitForSeconds(3);
            Rig.AddForce(vec * _meleeForce, ForceMode2D.Force);
            yield return new WaitForSeconds(2.5f);

            //Se nao colidir em nada
            AnimScript.StunOff();
            EnableAgent();
            _isRolling = false;
            AnimScript.RollingOff();
            StartCoroutine("RollingAtkDelay");

        }
    }

    IEnumerator RollingAtkDelay()
    {
        yield return new WaitForSeconds(RollingAtkTimeDelay);
        _canRollingAtk = true;
    }

    IEnumerator StunDelay()
    {
        yield return new WaitForSeconds(3);
        AnimScript.StunOff();
        IsStunned = false;

    }

}
