using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Corn : Enemy
{
    //Delays
    [field: SerializeField] public float CobMissileAtkTimeDelay { get; set; }
    [field: SerializeField] public float FleeFromPlayerTimeDelay { get; set; }
    //Ranges
    [field: SerializeField] public float CobMissileAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }

    //Controles
    private bool _canCobMissileAtk { get; set; }
    [field: SerializeField] private GameObject _aim { get; set; }
    [field: SerializeField] private GameObject _projectile { get; set; }
    private Vector3 _targetPosition { get; set; }
    private bool _canFleeFromPlayer { get; set; }

    //Outros
    public CornAnim AnimScript { get; set; }
    [field: SerializeField] private int _rangedForce { get; set; }


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<CornAnim>();
        StartCoroutine("CobMissileAtkDelay");
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
        Collider2D cobMissileAtk = Physics2D.OverlapCircle(transform.position, CobMissileAtkRange, PlayerLayer);
        Collider2D fleeFromPlayer = Physics2D.OverlapCircle(transform.position, _agent.stoppingDistance, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (cobMissileAtk != null && _canCobMissileAtk && _agent.isActiveAndEnabled)
        {
            _canCobMissileAtk = false;
            ReturnToStartTimer = 0;
            StartCoroutine("CobMissileAttack");
        }
        else if (fleeFromPlayer != null && _canFleeFromPlayer && _agent.isActiveAndEnabled)
        {
            _agent.SetDestination(-(_player.transform.position - transform.position) * 2);
            _canFleeFromPlayer = false;
            ReturnToStartTimer = 0;
            StartCoroutine("FleeFromPlayer");
        }
        else if (setDestination != null && _agent.isActiveAndEnabled && cobMissileAtk == null && _canFleeFromPlayer)
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
        Gizmos.DrawWireSphere(transform.position, CobMissileAtkRange);
    }

    IEnumerator FleeFromPlayer()
    {
        yield return new WaitForSeconds(FleeFromPlayerTimeDelay);
        _canFleeFromPlayer = true;
    }



    //Coroutines
    IEnumerator CobMissileAtkDelay()
    {
        yield return new WaitForSeconds(CobMissileAtkTimeDelay);
        _canCobMissileAtk = true;
    }

    IEnumerator CobMissileAttack()
    {

        NavMeshHit hit;
        if (!_agent.Raycast(_player.transform.position, out hit))
        {
            _targetPosition = hit.position;
            LastMoveDirection = _agent.velocity;
            Vector2 vec = (_targetPosition - gameObject.transform.position).normalized;


            DisableAgent();
            AnimScript.CobMissileAttack(LastMoveDirection.x, LastMoveDirection.y);
            GameObject projectile = Instantiate(_projectile, _aim.transform.position, _aim.transform.rotation);
            if (Mathf.Abs(vec.x) < Mathf.Abs(vec.y))
            {
                projectile.GetComponent<CornProjectile>().ChangeSprite();
            }
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            projectile.GetComponent<CornProjectile>().Damage = Power / 2;
            projectile.GetComponent<Rigidbody2D>().AddForce(vec * _rangedForce, ForceMode2D.Force);

            yield return new WaitForSeconds(0.2f);
            EnableAgent();

        }

        StartCoroutine("CobMissileAtkDelay");



    }

}
