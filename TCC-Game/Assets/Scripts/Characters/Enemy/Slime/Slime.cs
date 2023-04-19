using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    [field: SerializeField] private GameObject _aim { get; set; }
    [field: SerializeField] private GameObject _projectile { get; set; }
    private Vector3 _targetPosition { get; set; }

    //Outros
    public SlimeAnim AnimScript { get; set; }
    [field: SerializeField] private int _meleeForce { get; set; }
    [field: SerializeField] private int _rangedForce { get; set; }


    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<SlimeAnim>();
        StartCoroutine("MoveAtkDelay");
        StartCoroutine("ProjectileAtkDelay");
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
        Collider2D projectileAtk = Physics2D.OverlapCircle(transform.position, ProjectileAtkRange, PlayerLayer);
        AnimScript.Move(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (moveAtk != null && _canMoveAtk && _agent.isActiveAndEnabled)
        {
            _canMoveAtk = false;
            ReturnToStartTimer = 0;
            MoveAttack();
        }
        else if (projectileAtk != null && _canProjectileAtk && _agent.isActiveAndEnabled)
        {
            _canProjectileAtk = false;
            ReturnToStartTimer = 0;
            StartCoroutine("ProjectileAttack");
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
        Gizmos.DrawWireSphere(transform.position, ProjectileAtkRange);
        Gizmos.DrawWireSphere(transform.position, MoveAtkRange);

    }

    void MoveAttack()
    {
        LastMoveDirection = _agent.velocity;
        DisableAgent();
        Rig.AddForce(LastMoveDirection * _meleeForce, ForceMode2D.Force);

        StartCoroutine("MoveAtkDelay");
        AnimScript.MoveAttack(LastMoveDirection.x, LastMoveDirection.y);
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

    IEnumerator ProjectileAttack()
    {
        NavMeshHit hit;
        if (!_agent.Raycast(_player.transform.position, out hit))
        {
            _targetPosition = hit.position;
            LastMoveDirection = _agent.velocity;
            DisableAgent();
            AnimScript.ProjectileAttack(LastMoveDirection.x, LastMoveDirection.y);

            yield return new WaitForSeconds(0.5f);

            GameObject projectile = Instantiate(_projectile, _aim.transform.position, _aim.transform.rotation);
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            projectile.GetComponent<SlimeProjectile>().Damage = Power / 2;
            projectile.GetComponent<Rigidbody2D>().AddForce((_targetPosition - projectile.transform.position).normalized * _rangedForce, ForceMode2D.Force);

            yield return new WaitForSeconds(1.0f);
            EnableAgent();

        }

        StartCoroutine("ProjectileAtkDelay");



    }

}
