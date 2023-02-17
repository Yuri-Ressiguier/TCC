using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public abstract class Enemy : Character
{
    protected NavMeshAgent _agent;
    [field: SerializeField] protected Player _player;
    [field: SerializeField] protected LayerMask PlayerLayer { get; set; }

    public override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
    }

    public override void TakeHit(float dmg)
    {
        base.TakeHit(dmg);
        EnableAgent();
        _agent.isStopped = false;
        _agent.SetDestination(_player.transform.position);
    }

    //Chamado pelas animações. Verificar se é boa prática
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
}
