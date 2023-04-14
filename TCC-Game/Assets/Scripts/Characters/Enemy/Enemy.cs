using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : Character
{
    protected NavMeshAgent _agent;
    protected Player _player;
    public Spawner Spawner { get; set; }
    [field: SerializeField] protected LayerMask PlayerLayer { get; set; }

    private float _returnToStartTimeDelay;
    public float ReturnToStartTimer { get; set; }

    [field: SerializeField] public GameObject Coin { get; set; }
    [field: SerializeField] public GameObject HealthPotion { get; set; }
    [field: SerializeField] public Image HealthBar { get; set; }



    public override void Start()
    {
        base.Start();
        _player = FindObjectOfType<Player>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.enabled = true;
        _returnToStartTimeDelay = 8;
        ReturnToStartTimer = 0;

    }

    public virtual void Update()
    {
        ReturnToStartTimer += Time.deltaTime;

        if (ReturnToStartTimer >= _returnToStartTimeDelay)
        {
            ReturnToStartTimer = 0;
            ReturnToStart();
        }
    }


    public virtual void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Player" && collision.collider == _player.MainCollider)
        {
            _player.TakeHit(Power);
        }

    }

    public override void TakeHit(float dmg)
    {
        base.TakeHit(dmg);
        HealthBar.fillAmount = Life / LifeCap;
        if (_agent.isActiveAndEnabled)
        {
            _agent.SetDestination(_player.transform.position);
        }

    }

    public override void Die()
    {
        GenerateExp();
        base.Die();
        Spawner.ObjectList.Remove(this);
        StartCoroutine("Drop");
    }


    protected void EnableAgent()
    {
        Rig.velocity = Vector2.zero;
        _agent.enabled = true;
        _agent.isStopped = false;
        Rig.isKinematic = true;
    }

    protected void DisableAgent()
    {
        _agent.enabled = false;
        Rig.isKinematic = false;
    }

    protected void PauseAgent()
    {
        _agent.isStopped = true;
    }

    protected void UnpauseAgent()
    {
        _agent.isStopped = false;
    }

    protected virtual void ReturnToStart()
    {
        if ((Mathf.Abs(transform.position.x - InitialPosition.x) < 0.5) && (Mathf.Abs(transform.position.y - InitialPosition.y) < 0.5))
        {
            float newXAxis = transform.position.x + Random.Range(-2, 2);
            float newYAxis = transform.position.y + Random.Range(-2, 2);
            _agent.SetDestination(new Vector2(newXAxis, newYAxis));
        }
        else
        {
            _agent.isStopped = false;
            Life = LifeCap;
            HealthBar.fillAmount = Life / LifeCap;
            _agent.SetDestination(InitialPosition);
        }

    }

    protected void GenerateExp()
    {
        _player.ReceiveExp((int)Mathf.Round((LifeCap + Power) / 2));
    }

    //Coroutines

    IEnumerator Drop()
    {
        yield return new WaitForSeconds(0.8f);
        int randInt = Random.Range(0, 10);

        switch (randInt)
        {
            case 0:
            case 1:
            case 2:
                GameObject healthPotion = Instantiate(HealthPotion, transform.position, transform.rotation);
                healthPotion.name = HealthPotion.name;
                break;
            default:
                GameObject coin = Instantiate(Coin, transform.position, transform.rotation);
                coin.name = Coin.name;
                coin.GetComponent<Coin>().Value = (int)Mathf.Round(((LifeCap + Power) / 2) / 2);
                break;
        }


    }

}
