using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [field: SerializeField] private GameObject _projectile_horizontal { get; set; }
    [field: SerializeField] private GameObject _projectile_vertical { get; set; }
    private Vector3 _targetPosition { get; set; }
    private bool _canFleeFromPlayer { get; set; }

    //Outros
    public Math_87_Anim AnimScript { get; set; }
    [field: SerializeField] private int _rangedForce { get; set; }

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
            //ELE NAO TA ANDANDO SÓ NA VERTICAL OU SO NA HORIZONTAL*

            Vector2 vec = -(_player.transform.position - transform.position) * 3;
            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                Vector2 vecx = new Vector2(vec.x, 0);
                _agent.SetDestination(vecx);
            }
            else
            {
                Vector2 vecy = new Vector2(0, vec.y);
                _agent.SetDestination(vecy);
            }

            _canFleeFromPlayer = false;
            ReturnToStartTimer = 0;
            StartCoroutine("FleeFromPlayer");
        }
        else if (setDestination != null && _agent.isActiveAndEnabled && arrowAtk == null && _canFleeFromPlayer)
        {
            //ELE NAO TA ANDANDO SÓ NA VERTICAL OU SO NA HORIZONTAL*
            ReturnToStartTimer = 0;
            Vector2 vec = _player.transform.position;
            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                _agent.SetDestination(new Vector2(vec.x, 0));
            }
            else
            {
                _agent.SetDestination(new Vector2(0, vec.y));
            }

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
            GameObject projectile;
            _targetPosition = hit.position;
            LastMoveDirection = _agent.velocity;
            Vector2 vec = (_targetPosition - gameObject.transform.position).normalized;
            DisableAgent();
            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                projectile = Instantiate(_projectile_horizontal, _aim.transform.position, _aim.transform.rotation);
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
                if (vec.x < 0)
                {
                    projectile.GetComponent<Arrow>().ChangeSprite();
                    AnimScript.ArrowAttack(LastMoveDirection.x, LastMoveDirection.y, 3);
                }
                else
                {
                    AnimScript.ArrowAttack(LastMoveDirection.x, LastMoveDirection.y, 1);
                }
            }
            else
            {
                projectile = Instantiate(_projectile_vertical, _aim.transform.position, _aim.transform.rotation);
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
                if (vec.y < 0)
                {
                    projectile.GetComponent<Arrow>().ChangeSprite();
                    AnimScript.ArrowAttack(LastMoveDirection.x, LastMoveDirection.y, 3);
                }
                else
                {
                    AnimScript.ArrowAttack(LastMoveDirection.x, LastMoveDirection.y, 1);
                }
            }


            projectile.GetComponent<Arrow>().Damage = Power / 2;
            projectile.GetComponent<Rigidbody2D>().AddForce(vec * _rangedForce, ForceMode2D.Force);

            yield return new WaitForSeconds(0.2f);
            EnableAgent();

        }

        StartCoroutine("ArrowAtkDelay");



    }
}
