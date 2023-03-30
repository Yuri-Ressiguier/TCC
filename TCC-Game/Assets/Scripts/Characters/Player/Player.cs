using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    //Status
    [field: SerializeField] public float Speed { get; set; }
    private float _energy { get; set; }
    private float _energyLimit { get; set; }
    private float _walkingMeleeEnergy { get; set; }
    private float _walkingDefenseEnergy { get; set; }
    private float _meleeEnergy { get; set; }
    private float _rangedAttackEnergy { get; set; }
    private float _defenseEnergy { get; set; }
    private float _interruptEnergy { get; set; }
    private int _lvl { get; set; }
    private int _lvlCap { get; set; }
    private int _expCap { get; set; }
    private int _exp { get; set; }

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

    private static Player _instance;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        _expCap = 20;
        _lvl = 1;
        _lvlCap = 20;

        //Energy
        _energyLimit = 100;
        _walkingMeleeEnergy = 40;
        _walkingDefenseEnergy = 40;
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
            UiController.UiInstance.EnergyBar.fillAmount = _energy / 100;
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
                    UiController.UiInstance.TxtCoins.text = Coins.ToString();
                    coin.Pick();
                    break;
                case "HealthPotion":
                    HealthPotions += 1;
                    UiController.UiInstance.TxtHealthPotions.text = HealthPotions.ToString();
                    Destroy(other.gameObject, 0);
                    break;
                default:
                    break;
            }
        }

        if (other.gameObject.tag == "Gate")
        {
            SaveState();
            Vector2 nextPos = other.gameObject.GetComponent<Gate>().NextMap();
            gameObject.transform.position = nextPos;
            LoadState();

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
        if (!IsStunned && UiController.UiInstance.BtnAttack.gameObject.activeSelf)
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
        if (!IsRangedModeOn && !IsStunned)
        {
            if (_isWalking)
            {
                WalkingDef();
            }
            else
            {
                Defense();
            }

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
            UiController.UiInstance.BtnHealthPotion.interactable = false;
            HealthPotions -= 1;
            UiController.UiInstance.TxtHealthPotions.text = HealthPotions.ToString();
            if (Life + HealthPotion.Value >= LifeCap)
            {
                Life = LifeCap;
            }
            Life += HealthPotion.Value;
            UiController.UiInstance.LifeBar.fillAmount = Life / LifeCap;
            StartCoroutine("HealDelay");
        }
    }

    void Move()
    {
        if (!DialogueManager.DialogueInstance.DialogueIsPlaying)
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
        else
        {
            AnimScript.MagnitudeZero();
        }

    }

    public override void TakeHit(float dmg)
    {
        base.TakeHit(dmg);
        UiController.UiInstance.LifeBar.fillAmount = Life / LifeCap;
    }

    void WalkingAtk()
    {
        if (_canAttackMelee)
        {
            if (_energy >= _walkingMeleeEnergy)
            {
                UiController.UiInstance.BtnAttack.interactable = false;
                AnimScript.WalkingAtk(_direction.x, _direction.y, _direction.magnitude);
                _energy -= _walkingMeleeEnergy;
                _canAttackMelee = false;
                StartCoroutine("MeleeAttackDelay");
            }
            else
            {
                UiController.UiInstance.EnergyWarning();
            }
        }
    }

    void WalkingDef()
    {
        if (_canDefense)
        {
            if (_energy >= _walkingDefenseEnergy)
            {
                UiController.UiInstance.BtnDefense.interactable = false;
                AnimScript.WalkingDef(_direction.x, _direction.y, _direction.magnitude);
                _energy -= _walkingDefenseEnergy;
                _canDefense = false;
                StartCoroutine("DefenseDelay");
            }
            else
            {
                UiController.UiInstance.EnergyWarning();
            }
        }
    }

    void Attack()
    {


        if (IsRangedModeOn)
        {
            if (_canAttackRanged)
            {
                if (_energy >= _rangedAttackEnergy)
                {
                    UiController.UiInstance.BtnAttack.interactable = false;
                    AnimScript.Attack();
                    _energy -= _rangedAttackEnergy;
                    GameObject selectedAim = _aims[RangedModeOrientation];
                    selectedAim.GetComponent<Aim>().Shoot(Power / 2);
                    _canAttackRanged = false;
                    StartCoroutine("RangedAttackDelay");
                }
                else
                {
                    UiController.UiInstance.EnergyWarning();
                }
            }
        }
        else
        {
            if (_canAttackMelee)
            {
                if (_energy >= _meleeEnergy)
                {
                    UiController.UiInstance.BtnAttack.interactable = false;
                    AnimScript.Attack();
                    _energy -= _meleeEnergy;
                    _canAttackMelee = false;
                    StartCoroutine("MeleeAttackDelay");
                }
                else
                {
                    UiController.UiInstance.EnergyWarning();
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
                UiController.UiInstance.BtnDefense.interactable = false;
                AnimScript.Defense();
                _energy -= _defenseEnergy;
                _canDefense = false;
                StartCoroutine("DefenseDelay");
            }
            else
            {
                UiController.UiInstance.EnergyWarning();
            }
        }
    }

    void Interrupt()
    {
        if (_canInterrupt)
        {
            if (_energy >= _interruptEnergy)
            {
                UiController.UiInstance.BtnInterrupt.interactable = false;
                AnimScript.Interrupt();
                _energy -= _interruptEnergy;
                _canInterrupt = false;
                StartCoroutine("InterruptDelay");
            }
            else
            {
                UiController.UiInstance.EnergyWarning();
            }
        }
    }


    void TurnRangedModeOn()
    {
        if (IsRangedModeOn)
        {
            UiController.UiInstance.ChangeAttackImg(true);
            IsRangedModeOn = false;
            AnimScript.TurnRangedModeOff();
        }
        else
        {
            UiController.UiInstance.ChangeAttackImg(false);
            IsRangedModeOn = true;
            AnimScript.TurnRangedModeOn();
        }
    }

    public void ReceiveExp(int exp)
    {
        if (exp + _exp >= _expCap)
        {
            int spareExp = exp + _exp - _expCap;
            LevelUp();
            _exp = spareExp;
            _expCap = (int)Mathf.Round(_expCap * 1.2f);
        }
        else
        {
            _exp += exp;
        }
        UiController.UiInstance.ExpBar.fillAmount = (float)Math.Round((float)_exp / _expCap, 2);
    }

    void LevelUp()
    {
        if (_lvl < _lvlCap)
        {
            _lvl++;
            Power += 3;
            LifeCap += 3;
            Life = LifeCap;
            UiController.UiInstance.TxtLvl.text = _lvl.ToString();
        }
    }


    void SaveState()
    {
        PlayerPrefs.SetFloat("Power", Power);
        PlayerPrefs.SetFloat("Life", Life);
        PlayerPrefs.SetFloat("LifeCap", LifeCap);
        PlayerPrefs.SetInt("Exp", _exp);
        PlayerPrefs.SetInt("ExpCap", _expCap);
        PlayerPrefs.SetInt("Lvl", _lvl);
        PlayerPrefs.SetInt("HealthPotions", HealthPotions);
        PlayerPrefs.SetInt("Coins", Coins);

    }

    void LoadState()
    {
        Power = PlayerPrefs.GetFloat("Power");
        Life = PlayerPrefs.GetFloat("Life");
        LifeCap = PlayerPrefs.GetFloat("LifeCap");
        _exp = PlayerPrefs.GetInt("Exp");
        _expCap = PlayerPrefs.GetInt("ExpCap");
        _lvl = PlayerPrefs.GetInt("Lvl");
        HealthPotions = PlayerPrefs.GetInt("HealthPotions");
        Coins = PlayerPrefs.GetInt("Coins");

        //Atualizar UI
        UiController.UiInstance.LifeBar.fillAmount = Life / LifeCap;
        UiController.UiInstance.ExpBar.fillAmount = (float)Math.Round((float)_exp / _expCap, 2);
        UiController.UiInstance.TxtLvl.text = _lvl.ToString();
        UiController.UiInstance.TxtHealthPotions.text = HealthPotions.ToString();
        UiController.UiInstance.TxtCoins.text = Coins.ToString();


    }

    //Coroutines

    IEnumerator HealDelay()
    {
        yield return new WaitForSeconds(HealTimeDelay);
        _canHeal = true;
        UiController.UiInstance.BtnHealthPotion.interactable = true;
    }


    IEnumerator MeleeAttackDelay()
    {
        yield return new WaitForSeconds(MeleeTimeDelay);
        _canAttackMelee = true;
        UiController.UiInstance.BtnAttack.interactable = true;
    }

    IEnumerator RangedAttackDelay()
    {
        yield return new WaitForSeconds(RangedTimeDelay);
        _canAttackRanged = true;
        UiController.UiInstance.BtnAttack.interactable = true;
    }

    IEnumerator DefenseDelay()
    {
        yield return new WaitForSeconds(DefenseTimeDelay);
        _canDefense = true;
        UiController.UiInstance.BtnDefense.interactable = true;
    }

    IEnumerator InterruptDelay()
    {
        yield return new WaitForSeconds(InterruptTimeDelay);
        _canInterrupt = true;
        UiController.UiInstance.BtnInterrupt.interactable = true;
    }

}
