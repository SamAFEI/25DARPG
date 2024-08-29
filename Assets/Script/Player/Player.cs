using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : Entity
{
    public PlayerInput input { get; set; }
    public UI_PlayerStatus uiPlayerStatus { get; private set; }
    public UI_PlayerHint uiPlayerHint { get; private set; }
    #region FSM States
    public PlayerStateIdle idleState { get; set; }
    public PlayerStateRun runState { get; set; }
    public PlayerStateSword swordState { get; set; }
    public PlayerStateDash dashState { get; set; }
    public PlayerStateStun stunState { get; set; }
    public PlayerStateParry parryState { get; set; }
    public PlayerStateHurt hurtState { get; set; }
    public PlayerStateSex sexState { get; set; }
    #endregion
    public new PlayerData Data => (PlayerData)base.Data;
    public bool CanMovement { get { return !input.IsAttacking && !input.IsDashing && !input.IsParrying && !IsStunning && !IsSexing; } }
    public Vector3 MoveInput { get { return input.MoveInput; } }
    public SpriteLibraryAsset SLAssetNormal;
    public SpriteLibraryAsset SLAssetBreak1;
    public SpriteLibraryAsset SLAssetBreak2;
    public int hurtCount;

    protected override void Awake()
    {
        MaxHp = 100;
        CurrentHp = MaxHp;
        AttackDamage = 10;
        base.Awake();
        input = GetComponent<PlayerInput>();
        uiPlayerHint = GetComponentInChildren<UI_PlayerHint>();
        idleState = new PlayerStateIdle(this, FSM, "Idle");
        runState = new PlayerStateRun(this, FSM, "Run");
        swordState = new PlayerStateSword(this, FSM, "Sword1");
        dashState = new PlayerStateDash(this, FSM, "Dash");
        stunState = new PlayerStateStun(this, FSM, "Stun");
        parryState = new PlayerStateParry(this, FSM, "Parry");
        hurtState = new PlayerStateHurt(this, FSM, "Hurt");
        sexState = new PlayerStateSex(this, FSM, "Sex");
        FSM.InitState(idleState);
    }

    protected override void Start()
    {
        base.Start();
        uiPlayerStatus = GameObject.FindObjectOfType<UI_PlayerStatus>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); 
        CheckCamera();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyAttack")
        {
            if (IsHurting || IsStunning || IsSuperArmeding) { return; }
            Enemy _enemy = other.GetComponentInParent<Enemy>();
            float faceDir = rb.mass * 5;
            if (_enemy.transform.position.x > transform.position.x)
            {
                faceDir *= -1;
            }
            rb.AddForce(new Vector3(faceDir, 0, 0), ForceMode.Impulse);
            Hurt(_enemy.AttackDamage);
        }
    }

    #region Hurt
    public override IEnumerator HurtFlasher()
    {
        SetFlashColor();
        while (IsHurting)
        {
            SetFlashAmount(1);
            yield return new WaitForSeconds(.1f);
            SetFlashAmount(0);
            yield return new WaitForSeconds(.1f);
        }
    }

    public virtual void Hurt(float _damage, bool _isHeaveyAttack = false)
    {
        //DoDamageHp
        if (_damage > 0)
        {
            if (IsHurting || IsStunning || IsSuperArmeding) { return; }
            CameraManager.Instance.Shake(3f, 0.1f);
            LastHurtTime = Data.hurtResetTime;
            StartCoroutine(HurtFlasher());
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
            hurtCount++;
        }
        //HealHp
        else
        {
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
        }
    }

    public override void SexHurt()
    {
        if (!IsSexing) { return; }
        CameraManager.Instance.Shake(3f, 0.1f);
        foreach (Enemy _enemy in GameManager.Instance.sexEnemies)
        {
            float _damage = 0;
            _damage = _enemy.AttackDamage;
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
        }
    }
    #endregion

    #region Sex
    public void SearchSexEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Enemy enemy = collider.GetComponentInParent<Enemy>();
                sexAnimName = enemy.sexAnimName;
                IsSexing = true;
                StartCoroutine(Resist());
                GameManager.AddSexEnemies(enemy);
                break;
            }
        }
    }

    public IEnumerator Resist()
    {
        int _count = 1;
        float _currentInput;
        float _nextInput = 0;
        while (IsSexing)
        {
            if (MoveInput.x != 0)
            {
                _currentInput = MoveInput.x;
                if (_currentInput == _nextInput) 
                {
                    _count++;
                    if (_count == 10)
                    {
                        IsSexing = false;
                        yield break;
                    }
                }
                _nextInput = _currentInput * -1;
            }
            yield return null;
        }
    }
    #endregion

    public void CheckCamera()
    {
        if (transform.position.z >= 30)
        {
            CameraManager.Instance.ChangeCamera(1);
        }
    }
}
