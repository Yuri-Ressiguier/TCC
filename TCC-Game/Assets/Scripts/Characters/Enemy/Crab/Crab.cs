using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crab : Enemy
{
    //Delays
    [field: SerializeField] public float MoveAtkTimeDelay { get; set; }
    [field: SerializeField] public float WaterAtkTimeDelay { get; set; }


    //Ranges
    [field: SerializeField] public float MoveAtkRange { get; set; }
    [field: SerializeField] public float WaterAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }


    //Controles
    private bool _canMoveAtk { get; set; }
    private bool _canWaterAtk { get; set; }
    [field: SerializeField] private GameObject _projectile { get; set; }
    private Vector3 _targetPosition { get; set; }

    //SFX
    [field: SerializeField] private AudioClip _sfx { get; set; }
    private ActorSFX _actorSFX { get; set; }

    //Outros
    public CrabAnim AnimScript { get; set; }
    [field: SerializeField] private int _rangedForce { get; set; }


    public override void Start()
    {
        base.Start();
        _actorSFX = GetComponent<ActorSFX>();
        AnimScript = GetComponent<CrabAnim>();
        StartCoroutine("MoveAtkDelay");
        StartCoroutine("WaterAtkDelay");
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
        Collider2D moveAtk = Physics2D.OverlapCircle(transform.position, MoveAtkRange, PlayerLayer);
        Collider2D waterAtk = Physics2D.OverlapCircle(transform.position, WaterAtkRange, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, Direction(), _agent.velocity.magnitude);

        if (moveAtk != null && _canMoveAtk && _agent.isActiveAndEnabled)
        {
            _canMoveAtk = false;
            ReturnToStartTimer = 0;
            MoveAttack();
        }
        else if (waterAtk != null && _canWaterAtk && _agent.isActiveAndEnabled)
        {
            _canWaterAtk = false;
            _canWaterAtk = false;
            ReturnToStartTimer = 0;
            StartCoroutine("WaterAttack");
        }
        else if (setDestination != null && _agent.isActiveAndEnabled)
        {
            ReturnToStartTimer = 0;
            _agent.SetDestination(new Vector2(_player.transform.position.x, InitialPosition.y));
        }
        else
        {
            //Perambular

        }
    }

    void MoveAttack()
    {
        LastMoveDirection = _agent.velocity;
        PauseAgent();
        StartCoroutine("MoveAtkDelay");
        AnimScript.MoveAttack(LastMoveDirection.x, Direction());
    }

    int Direction()
    {
        NavMeshHit hit;
        int direction;
        if (!_agent.Raycast(_player.transform.position, out hit))
        {
            _targetPosition = hit.position;
            Vector2 vec = (_targetPosition - gameObject.transform.position).normalized;
            if (vec.y > 0)
            {
                direction = 0;
            }
            else
            {
                direction = 2;
            }
        }
        else
        {
            direction = 0;
        }

        return direction;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, SetDestinationRange);
        Gizmos.DrawWireSphere(transform.position, WaterAtkRange);
        Gizmos.DrawWireSphere(transform.position, MoveAtkRange);

    }

    protected override void ReturnToStart()
    {
        if ((Mathf.Abs(transform.position.x - InitialPosition.x) < 1))
        {
            float newXAxis = transform.position.x + Random.Range(-5, 5);
            _agent.SetDestination(new Vector2(newXAxis, InitialPosition.y));
        }
        else
        {
            _agent.isStopped = false;
            Life = LifeCap;
            HealthBar.fillAmount = Life / LifeCap;
            _agent.SetDestination(new Vector2(InitialPosition.x, InitialPosition.y));
        }
    }

    //Coroutines

    IEnumerator MoveAtkDelay()
    {
        yield return new WaitForSeconds(MoveAtkTimeDelay);
        _canMoveAtk = true;
    }
    IEnumerator WaterAtkDelay()
    {
        yield return new WaitForSeconds(WaterAtkTimeDelay);
        _canWaterAtk = true;
    }

    IEnumerator WaterAttack()
    {
        NavMeshHit hit;
        if (!_agent.Raycast(_player.transform.position, out hit))
        {
            if (Mathf.Round(_agent.gameObject.transform.position.x) == Mathf.Round(_player.transform.position.x))
            {
                _targetPosition = hit.position;
                Vector2 vec = (_targetPosition - gameObject.transform.position).normalized;

                yield return new WaitForSeconds(0.3f);

                GameObject projectile = Instantiate(_projectile, transform.position, transform.rotation);
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
                _actorSFX.PlaySFX(_sfx);
                projectile.GetComponent<CrabProjectile>().Damage = Power / 2;
                if (vec.y > 0)
                {
                    projectile.GetComponent<CrabProjectile>().AnimUp();
                }
                else
                {
                    projectile.GetComponent<CrabProjectile>().AnimDown();
                }
                projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, vec.y) * _rangedForce, ForceMode2D.Force);
            }
        }
        StartCoroutine("WaterAtkDelay");
    }

}
