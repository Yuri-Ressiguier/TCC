using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Butterfly : Enemy
{
    //Delays
    [field: SerializeField] public float StingAtkTimeDelay { get; set; }
    [field: SerializeField] public float WebBombAtkTimeDelay { get; set; }

    //Ranges
    [field: SerializeField] public float StingAtkRange { get; set; }
    [field: SerializeField] public float WebBombAtkRange { get; set; }
    [field: SerializeField] public float SetDestinationRange { get; set; }

    //Controles
    private bool _canStingAtk { get; set; }
    private bool _canWebBombAtk { get; set; }
    [field: SerializeField] private GameObject _aim { get; set; }
    [field: SerializeField] private GameObject _projectile { get; set; }
    private Vector3 _targetPosition { get; set; }

    //Outros
    public ButterflyAnim AnimScript { get; set; }
    [field: SerializeField] private int _meleeForce { get; set; }
    [field: SerializeField] private int _rangedForce { get; set; }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<ButterflyAnim>();
        StartCoroutine("StingAtkDelay");
        StartCoroutine("WebBombAtkDelay");
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
        Collider2D stingAtk = Physics2D.OverlapCircle(transform.position, StingAtkRange, PlayerLayer);
        Collider2D webBombAtk = Physics2D.OverlapCircle(transform.position, WebBombAtkRange, PlayerLayer);
        AnimScript.Fly(_agent.velocity.x, _agent.velocity.y, _agent.velocity.magnitude);

        if (stingAtk != null && _canStingAtk && _agent.isActiveAndEnabled)
        {
            _canStingAtk = false;
            ReturnToStartTimer = 0;
            StingAttack();
        }
        else if (webBombAtk != null && _canWebBombAtk && _agent.isActiveAndEnabled)
        {
            _canWebBombAtk = false;
            ReturnToStartTimer = 0;
            StartCoroutine("WebBombAttack");
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
        Gizmos.DrawWireSphere(transform.position, WebBombAtkRange);
        Gizmos.DrawWireSphere(transform.position, StingAtkRange);

    }

    void StingAttack()
    {
        LastMoveDirection = _agent.velocity;
        DisableAgent();
        Rig.AddForce(LastMoveDirection * _meleeForce, ForceMode2D.Force);

        StartCoroutine("StingAtkDelay");
        AnimScript.StingAttack(LastMoveDirection.x, LastMoveDirection.y);
    }

    //Coroutines

    IEnumerator StingAtkDelay()
    {
        yield return new WaitForSeconds(StingAtkTimeDelay);
        _canStingAtk = true;
    }
    IEnumerator WebBombAtkDelay()
    {
        yield return new WaitForSeconds(WebBombAtkTimeDelay);
        _canWebBombAtk = true;
    }

    IEnumerator WebBombAttack()
    {
        NavMeshHit hit;
        if (!_agent.Raycast(_player.transform.position, out hit))
        {
            _targetPosition = hit.position;
            LastMoveDirection = _agent.velocity;
            DisableAgent();
            AnimScript.WebBombAttack(LastMoveDirection.x, LastMoveDirection.y);
            GameObject projectile = Instantiate(_projectile, _aim.transform.position, _aim.transform.rotation);
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            projectile.GetComponent<ButterflyProjectile>().Damage = Power / 2;
            projectile.GetComponent<Rigidbody2D>().AddForce((_targetPosition - projectile.transform.position).normalized * _rangedForce, ForceMode2D.Force);

            yield return new WaitForSeconds(0.2f);
            EnableAgent();

        }

        StartCoroutine("WebBombAtkDelay");



    }
}
