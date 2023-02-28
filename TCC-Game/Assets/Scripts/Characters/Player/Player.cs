using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    //Status
    [field: SerializeField] public float Speed { get; set; }
    private float _energy { get; set; }
    private float _energyLimit { get; set; }
    private float _walkingMeleeEnergy { get; set; }
    private float _meleeEnergy { get; set; }
    private float _rangedAttackEnergy { get; set; }
    private float _defenseEnergy { get; set; }
    private float _interruptEnergy { get; set; }

    //Delays
    [field: SerializeField] private float MeleeTimeDelay { get; set; }
    [field: SerializeField] private float RangedTimeDelay { get; set; }
    [field: SerializeField] private float DefenseTimeDelay { get; set; }
    [field: SerializeField] private float InterruptTimeDelay { get; set; }
    [field: SerializeField] private float HealTimeDelay { get; set; }


    //Controles
    private Vector2 _direction { get; set; }
    private bool _canAttackMelee { get; set; }
    private bool _canAttackRanged { get; set; }
    private bool _canDefense { get; set; }
    private bool _canInterrupt { get; set; }
    private bool _canHeal { get; set; }
    private bool _isWalking { get; set; }
    public bool IsRangedModeOn { get; set; }
    public int RangedModeOrientation { get; set; }


    //Outros
    [field: SerializeField] public List<GameObject> _aims { get; set; }
    public PlayerAnim AnimScript { get; set; }
    [field: SerializeField] public BoxCollider2D MainCollider { get; set; }
    public int Coins { get; set; }
    public int HealthPotions { get; set; }




    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        AnimScript = GetComponent<PlayerAnim>();
        _canAttackMelee = true;
        _canAttackRanged = true;
        _canDefense = true;
        _canInterrupt = true;
        IsRangedModeOn = false;

        //Energy
        _energyLimit = 100;
        _walkingMeleeEnergy = 40;
        _meleeEnergy = 30;
        _defenseEnergy = 30;
        _rangedAttackEnergy = 50;
        _interruptEnergy = 30;

    }

    // Update is called once per frame
    void Update()
    {
        if (_energy < _energyLimit)
        {
            _energy += 20 * Time.deltaTime;
            GameController.GCInstance.EnergyBar.fillAmount = _energy / 100;
        }

    }

    void FixedUpdate()
    {
        Move();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            //Se nao cair no case, verificar se o nome tem (clone)
            switch (other.gameObject.name)
            {
                case "Coin":
                    Coin coin = other.gameObject.GetComponent<Coin>();
                    Coins += coin.Value;
                    GameController.GCInstance.TxtCoins.text = Coins.ToString();
                    coin.Pick();
                    break;
                case "HealthPotion":
                    HealthPotions += 1;
                    GameController.GCInstance.TxtHealthPotions.text = HealthPotions.ToString();
                    Destroy(other.gameObject, 0);
                    break;
                default:
                    break;
            }
        }
    }

    public void MoveInput(InputAction.CallbackContext value)
    {
        if (!IsStunned)
        {
            //Movimentação
            float moveX = value.ReadValue<Vector2>().x;
            float moveY = value.ReadValue<Vector2>().y;

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
        }
    }


    public void AttackInput()
    {
        if (!IsStunned)
        {
            if (_isWalking)
            {
                if (!IsRangedModeOn)
                {
                    WalkingAtk();
                }

            }
            else
            {
                Attack();
            }
        }
    }

    public void DefenseInput()
    {
        if (!_isWalking && !IsRangedModeOn && !IsStunned)
        {
            Defense();
        }
    }

    public void InterruptInput()
    {
        if (!_isWalking && !IsRangedModeOn && !IsStunned)
        {
            Interrupt();
        }
    }

    public void RangedModeInput()
    {
        if (!_isWalking && !IsStunned)
        {
            TurnRangedModeOn();
        }
    }

    public void Heal()
    {
        if (HealthPotions > 0)
        {
            GameController.GCInstance.BtnHealthPotion.interactable = false;
            HealthPotions -= 1;
            GameController.GCInstance.TxtHealthPotions.text = HealthPotions.ToString();
            if (Life + HealthPotion.Value >= 100)
            {
                Life = 100;
            }
            Life += HealthPotion.Value;
            GameController.GCInstance.LifeBar.fillAmount = Life / 100;
            StartCoroutine("HealDelay");
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
        GameController.GCInstance.LifeBar.fillAmount = Life / 100;
    }

    void WalkingAtk()
    {
        if (_canAttackMelee)
        {
            if (_energy >= _walkingMeleeEnergy)
            {
                GameController.GCInstance.BtnAttack.interactable = false;
                AnimScript.WalkingAtk(_direction.x, _direction.y, _direction.magnitude);
                _energy -= _walkingMeleeEnergy;
                _canAttackMelee = false;
                StartCoroutine("MeleeAttackDelay");
            }
            else
            {
                GameController.GCInstance.EnergyWarming();
            }
        }
    }

    void Attack()
    {


        if (IsRangedModeOn)
        {
            if (_canAttackRanged)
            {
                if (_energy >= _walkingMeleeEnergy)
                {
                    GameController.GCInstance.BtnAttack.interactable = false;
                    AnimScript.Attack();
                    _energy -= _rangedAttackEnergy;
                    GameObject selectedAim = _aims[RangedModeOrientation];
                    selectedAim.GetComponent<Aim>().Shoot(Power / 2);
                    _canAttackRanged = false;
                    StartCoroutine("RangedAttackDelay");
                }
                else
                {
                    GameController.GCInstance.EnergyWarming();
                }
            }
        }
        else
        {
            if (_canAttackMelee)
            {
                if (_energy >= _meleeEnergy)
                {
                    GameController.GCInstance.BtnAttack.interactable = false;
                    AnimScript.Attack();
                    _energy -= _meleeEnergy;
                    _canAttackMelee = false;
                    StartCoroutine("MeleeAttackDelay");
                }
                else
                {
                    GameController.GCInstance.EnergyWarming();
                }
            }
        }
    }

    void Defense()
    {
        if (_canDefense)
        {
            if (_energy >= _defenseEnergy)
            {
                GameController.GCInstance.BtnDefense.interactable = false;
                AnimScript.Defense();
                _energy -= _defenseEnergy;
                _canDefense = false;
                StartCoroutine("DefenseDelay");
            }
            else
            {
                GameController.GCInstance.EnergyWarming();
            }
        }
    }

    void Interrupt()
    {
        if (_canInterrupt)
        {
            if (_energy >= _interruptEnergy)
            {
                GameController.GCInstance.BtnInterrupt.interactable = false;
                AnimScript.Interrupt();
                _energy -= _interruptEnergy;
                _canInterrupt = false;
                StartCoroutine("InterruptDelay");
            }
            else
            {
                GameController.GCInstance.EnergyWarming();
            }
        }
    }


    void TurnRangedModeOn()
    {
        if (IsRangedModeOn)
        {
            GameController.GCInstance.ChangeAttackImg(true);
            IsRangedModeOn = false;
            AnimScript.TurnRangedModeOff();
        }
        else
        {
            GameController.GCInstance.ChangeAttackImg(false);
            IsRangedModeOn = true;
            AnimScript.TurnRangedModeOn();
        }
    }

    //Coroutines

    IEnumerator HealDelay()
    {
        yield return new WaitForSeconds(HealTimeDelay);
        _canHeal = true;
        GameController.GCInstance.BtnHealthPotion.interactable = true;
    }


    IEnumerator MeleeAttackDelay()
    {
        yield return new WaitForSeconds(MeleeTimeDelay);
        _canAttackMelee = true;
        GameController.GCInstance.BtnAttack.interactable = true;
    }

    IEnumerator RangedAttackDelay()
    {
        yield return new WaitForSeconds(RangedTimeDelay);
        _canAttackRanged = true;
        GameController.GCInstance.BtnAttack.interactable = true;
    }

    IEnumerator DefenseDelay()
    {
        yield return new WaitForSeconds(DefenseTimeDelay);
        _canDefense = true;
        GameController.GCInstance.BtnDefense.interactable = true;
    }

    IEnumerator InterruptDelay()
    {
        yield return new WaitForSeconds(InterruptTimeDelay);
        _canInterrupt = true;
        GameController.GCInstance.BtnInterrupt.interactable = true;
    }

}
