using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Player : Character
{
    //Status
    [field: SerializeField] public float Speed { get; set; }

    //Delays
    [field: SerializeField] private float MeleeTimeDelay { get; set; }
    [field: SerializeField] private float RangedTimeDelay { get; set; }
    [field: SerializeField] private float DefenseTimeDelay { get; set; }
    [field: SerializeField] private float InterruptTimeDelay { get; set; }


    //Controles
    private Vector2 _direction { get; set; }
    private bool _canAttackMelee { get; set; }
    private bool _canAttackRanged { get; set; }
    private bool _canDefense { get; set; }
    private bool _canInterrupt { get; set; }
    private bool _isWalking { get; set; }
    public bool IsRangedModeOn { get; set; }
    public int RangedModeOrientation { get; set; }


    //Outros
    [field: SerializeField] public List<GameObject> _aims { get; set; }
    public PlayerAnim AnimScript { get; set; }
    [field: SerializeField] public BoxCollider2D MainCollider { get; set; }




    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<PlayerAnim>();
        _canAttackMelee = true;
        _canAttackRanged = true;
        _canDefense = true;
        _canInterrupt = true;
        _canTakeHit = true;
        IsRangedModeOn = false;
        IsStunned = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStunned)
        {
            ProcessInputs();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        //Movimentação
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX == 0 && moveY == 0)
        {
            _isWalking = false;
            if (_direction.x != 0 || _direction.y != 0)
            {
                LastMoveDirection = _direction;
            }
        }
        else
        {
            _isWalking = true;
        }

        _direction = new Vector2(moveX, moveY);

        //Attack
        if (Input.GetKeyDown(KeyCode.Comma) && !_isWalking)
        {
            Attack();
        }

        //Defense
        if (Input.GetKeyDown(KeyCode.Period) && !_isWalking && !IsRangedModeOn)
        {
            Defense();
        }

        //Interrupt
        if (Input.GetKeyDown(KeyCode.Q) && !_isWalking && !IsRangedModeOn)
        {
            Interrupt();
        }

        //RangedMode
        if (Input.GetKeyDown(KeyCode.G) && !_isWalking)
        {
            TurnRangedModeOn();
        }

    }

    void Move()
    {

        if (IsRangedModeOn)
        {
            //Horizontal
            if (RangedModeOrientation == 0 || RangedModeOrientation == 2)
            {
                Rig.MovePosition(Rig.position + new Vector2(_direction.x, 0) * Time.fixedDeltaTime);
                AnimScript.MoveRanged(RangedModeOrientation, _direction.x, _direction.y, _direction.magnitude);
            }
            //Vertical
            else
            {
                Rig.MovePosition(Rig.position + new Vector2(0, _direction.y) * Time.fixedDeltaTime);
                AnimScript.MoveRanged(RangedModeOrientation, _direction.x, _direction.y, _direction.magnitude);
            }
        }
        else
        {
            //if (Math.Abs(Direction.y) >= Math.Abs(Direction.x))
            //{
            //    Rig.MovePosition(Rig.position + new Vector2(0, Direction.y) * Speed * Time.fixedDeltaTime);
            //}
            //else
            //{
            //    Rig.MovePosition(Rig.position + new Vector2(Direction.x, 0) * Speed * Time.fixedDeltaTime);
            //}
            Rig.MovePosition(Rig.position + _direction.normalized * Speed * Time.fixedDeltaTime);

            AnimScript.Move(_direction.x, _direction.y, _direction.magnitude, LastMoveDirection.x, LastMoveDirection.y);
        }

    }

    public override void TakeHit(float dmg)
    {
        base.TakeHit(dmg);
    }

    void Attack()
    {


        if (IsRangedModeOn)
        {
            if (_canAttackRanged)
            {
                AnimScript.Attack();
                GameObject selectedAim = _aims[RangedModeOrientation];
                selectedAim.GetComponent<Aim>().Shoot(Power / 3);
                _canAttackRanged = false;
                StartCoroutine("RangedAttackDelay");
            }
        }
        else
        {
            if (_canAttackMelee)
            {
                AnimScript.Attack();
                _canAttackMelee = false;
                StartCoroutine("MeleeAttackDelay");
            }

        }
    }

    void Defense()
    {
        if (_canDefense)
        {
            AnimScript.Defense();
            _canDefense = false;
            StartCoroutine("DefenseDelay");
        }

    }

    void Interrupt()
    {
        if (_canInterrupt)
        {
            AnimScript.Interrupt();
            _canInterrupt = false;
            StartCoroutine("InterruptDelay");
        }
    }


    void TurnRangedModeOn()
    {
        if (IsRangedModeOn)
        {
            IsRangedModeOn = false;
            AnimScript.TurnRangedModeOff();
        }
        else
        {
            IsRangedModeOn = true;
            AnimScript.TurnRangedModeOn();
        }
    }

    //Coroutines


    IEnumerator MeleeAttackDelay()
    {
        yield return new WaitForSeconds(MeleeTimeDelay);
        _canAttackMelee = true;
    }

    IEnumerator RangedAttackDelay()
    {
        yield return new WaitForSeconds(RangedTimeDelay);
        _canAttackRanged = true;
    }

    IEnumerator DefenseDelay()
    {
        yield return new WaitForSeconds(DefenseTimeDelay);
        _canDefense = true;
    }

    IEnumerator InterruptDelay()
    {
        yield return new WaitForSeconds(InterruptTimeDelay);
        _canInterrupt = true;
    }

}
