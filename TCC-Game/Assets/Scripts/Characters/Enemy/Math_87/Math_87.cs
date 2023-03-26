using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Math_87 : Enemy
{
    //Delays
    [field: SerializeField] public float ArrowAtkTimeDelay { get; set; }
    [field: SerializeField] public float FleeFromPlayerTimeDelay { get; set; }
    //Ranges
    [field: SerializeField] public float ArrowAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }

    //Controles
    private bool _canArrowAtk { get; set; }
    [field: SerializeField] private GameObject _aim { get; set; }
    [field: SerializeField] private GameObject _projectile { get; set; }
    private Vector3 _targetPosition { get; set; }
    private bool _canFleeFromPlayer { get; set; }

    //Outros
    public Math_87_Anim AnimScript { get; set; }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<Math_87_Anim>();
        StartCoroutine("ArrowAtkDelay");
        Rig.isKinematic = true;
        _canFleeFromPlayer = true;

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
        Collider2D arrowAtk = Physics2D.OverlapCircle(transform.position, ArrowAtkRange, PlayerLayer);
        Collider2D fleeFromPlayer = Physics2D.OverlapCircle(transform.position, _agent.stoppingDistance, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (arrowAtk != null && _canArrowAtk && _agent.isActiveAndEnabled)
        {
            _canArrowAtk = false;
            ReturnToStartTimer = 0;
            StartCoroutine("ArrowAttack");
        }
        else if (fleeFromPlayer != null && _canFleeFromPlayer && _agent.isActiveAndEnabled)
        {
            _agent.SetDestination(-(_player.transform.position - transform.position) * 2);
            _canFleeFromPlayer = false;
            ReturnToStartTimer = 0;
            StartCoroutine("FleeFromPlayer");
        }
        else if (setDestination != null && _agent.isActiveAndEnabled && arrowAtk == null && _canFleeFromPlayer)
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
        Gizmos.DrawWireSphere(transform.position, ArrowAtkRange);
    }

    IEnumerator FleeFromPlayer()
    {
        yield return new WaitForSeconds(FleeFromPlayerTimeDelay);
        _canFleeFromPlayer = true;
    }



    //Coroutines
    IEnumerator ArrowAtkDelay()
    {
        yield return new WaitForSeconds(ArrowAtkTimeDelay);
        _canArrowAtk = true;
    }

    IEnumerator ArrowAttack()
    {

        NavMeshHit hit;
        if (!_agent.Raycast(_player.transform.position, out hit))
        {
            _targetPosition = hit.position;
            LastMoveDirection = _agent.velocity;
            Vector2 vec = (_targetPosition - gameObject.transform.position).normalized;
            DisableAgent();
            GameObject projectile = Instantiate(_projectile, _aim.transform.position, _aim.transform.rotation);
            if (vec.x < 0)
            {
                projectile.GetComponent<Arrow>().ChangeSprite();
                AnimScript.ArrowAttack(LastMoveDirection.x, LastMoveDirection.y, 3);
            }
            else
            {
                AnimScript.ArrowAttack(LastMoveDirection.x, LastMoveDirection.y, 1);
            }
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            projectile.GetComponent<Arrow>().Damage = Power / 2;
            projectile.GetComponent<Rigidbody2D>().AddForce(vec * 11, ForceMode2D.Force);

            yield return new WaitForSeconds(0.2f);
            EnableAgent();

        }

        StartCoroutine("ArrowAtkDelay");



    }
}
