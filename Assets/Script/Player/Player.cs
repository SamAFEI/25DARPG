using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : Entity
{
    #region Components
    public PlayerInput input { get; set; }
    public UI_PlayerStatus uiPlayerStatus { get; private set; }
    public UI_PlayerHint uiPlayerHint { get; private set; }
    #endregion

    #region FSM States
    public PlayerStateIdle idleState { get; set; }
    public PlayerStateRun runState { get; set; }
    public PlayerStateSword swordState { get; set; }
    public PlayerStateDash dashState { get; set; }
    public PlayerStateStun stunState { get; set; }
    public PlayerStateParry parryState { get; set; }
    public PlayerStateHurt hurtState { get; set; }
    public PlayerStateSex sexState { get; set; }
    public PlayerStateDie dieState { get; set; }
    #endregion
    public new PlayerData Data => (PlayerData)base.Data;
    public bool CanMovement { get { return !input.IsAttacking && !input.IsDashing && !input.IsParrying && !IsStunning && !IsSexing; } }
    public Vector3 MoveInput { get { return input.MoveInput; } }
    public SpriteLibraryAsset SLAssetNormal;
    public SpriteLibraryAsset SLAssetBreak1;
    public SpriteLibraryAsset SLAssetBreak2;
    public bool IsCounter;
    public bool IsBreak1;
    public bool IsBreak2;
    public string sexAnimName;
    public string testSexAnim;
    public int testSexAnimIndex;

    protected override void Awake()
    {
        base.Awake();
        MaxHp = Data.MaxHP;
        CurrentHp = MaxHp;
        AttackDamage = Data.AttackDamage;
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
        dieState = new PlayerStateDie(this, FSM, "Die");
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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            testSexAnimIndex++;
            if (testSexAnimIndex > 3) testSexAnimIndex = 1;
            testSexAnim = "OrcForeplay" + testSexAnimIndex.ToString().PadLeft(2, '0');
            sexAnimName = testSexAnim;
            IsSexing = true;
            Debug.Log(sexAnimName);
            FSM.ChangeState(sexState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            testSexAnimIndex++;
            if (testSexAnimIndex > 4) testSexAnimIndex = 1;
            testSexAnim = "OrcSex" + testSexAnimIndex.ToString().PadLeft(2, '0');
            sexAnimName = testSexAnim;
            IsSexing = true;
            Debug.Log(sexAnimName);
            FSM.ChangeState(sexState);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            //•Œß¿ª•¥±º
            if ((other.transform.position - entityCollider.ClosestPoint(other.transform.position)).magnitude > 0.3f) { return; }
            if (IsHurting || IsStunning || IsSuperArmeding) { return; }
            ProjectileBase _projectile = other.GetComponentInParent<ProjectileBase>();
            Repel(_projectile.transform.position);
            Hurt(_projectile.AttackDamage, _projectile.IsHeaveyAttack, _projectile.IsAttackBeDefended);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemyAttack")
        {
            Enemy _enemy = other.GetComponentInParent<Enemy>();
            if (input.CanCounter && _enemy.IsAttackBeDefended && _enemy.CanBeStunned)
            {
                IsCounter = true;
                _enemy.LastStunTime = 3f;
                return;
            }
            if (IsHurting || IsStunning || IsSuperArmeding || _enemy.CanDamage) { return; }
            if (_enemy.IsCatching)
            {
                if (!IsBreak1)
                {
                    sexAnimName = _enemy.Data.foreplayAnims[Random.Range(0, _enemy.Data.foreplayAnims.Count)].name;
                }
                else
                {
                    sexAnimName = _enemy.Data.sexAnims[Random.Range(0, _enemy.Data.sexAnims.Count)].name;
                }
                IsSexing = true;
                FSM.ChangeState(sexState);
                StartCoroutine(Resist());
                GameManager.AddSexEnemies(_enemy);
                return;
            }
            float damage = _enemy.AttackDamage * Random.Range(0.80f, 1.20f);
            Repel(_enemy.transform.position, _enemy.IsHeaveyAttack);
            Hurt(damage, _enemy.IsHeaveyAttack, _enemy.IsAttackBeDefended);
        }
    }

    public override void Die(float _delay = 0.8f)
    {
        input.inputHandle.Character.Disable();
        input.inputHandle.SexAction.Disable();
        UI_Canvas.Instance.FadeInUI_Die();
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

    public virtual void Hurt(float _damage, bool _isHeaveyAttack = false, bool _isBeDefend = false)
    {
        //DoDamageHp
        if (_damage > 0)
        {
            if (IsHurting || IsStunning || IsSuperArmeding) { return; }
            CameraManager.Shake(3f, 0.1f);
            LastHurtTime = Data.hurtResetTime;
            if (_isBeDefend)
            {
                _damage *= 0.8f;
                if (!_isHeaveyAttack) { LastHurtTime = 0f; }
            }
            StartCoroutine(HurtFlasher());
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
        }
        //HealHp
        else
        {
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth(5f);
            entityFX.DoPlayBuffFX(0);
        }
        if (_isHeaveyAttack)
        {
            LastStunTime = Data.hurtResetTime;
        }
    }

    public override void SexHurt()
    {
        if (!IsSexing) { return; }
        CameraManager.Shake(3f, 0.1f);
        foreach (Enemy _enemy in GameManager.Instance.sexEnemies)
        {
            float _damage = _enemy.AttackDamage;
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
        }
    }

    public void SetBreak(bool reset = false)
    {
        if (reset)
        {
            IsBreak1 = false;
            IsBreak2 = false;
            SetSpriteLibraryAsset(SLAssetNormal);
            return;
        }
        if (!IsBreak1)
        {
            IsBreak1 = true;
            SetSpriteLibraryAsset(SLAssetBreak1);
            return;
        }
        if (!IsBreak2)
        {
            IsBreak2 = true;
            SetSpriteLibraryAsset(SLAssetBreak2);
            return;
        }
    }

    public void Repel(Vector3 sourcePosition, bool isHeavyAttack = false)
    {
        Vector3 _vector = CheckRelativeVector(sourcePosition);
        CheckIsFacingRight(_vector.x > 0);
        float faceRight = _vector.x > 0 ? -1 : 1;
        faceRight *= rb.mass * 3;
        if (isHeavyAttack) { faceRight *= 5; }
        _vector = new Vector3(faceRight, 0, 0);
        _vector = CameraManager.GetDirectionByCamera(_vector);
        SetZeroVelocity();
        rb.AddForce(_vector, ForceMode.Impulse);
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
                sexAnimName = "";
                IsSexing = true;
                StartCoroutine(Resist());
                GameManager.AddSexEnemies(enemy);
                break;
            }
        }
    }

    /// <summary>
    /// ±√§„Sex
    /// </summary>
    /// <returns></returns>
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

    public void ItemAction(int _index)
    {
        Inventory inventory = UI_Shortcut.Instance.GetSlotItemData(_index);
        if (inventory == null || inventory.amount == 0) return;

        ItemData item = inventory.item;
        if (item.name == "HPPotion")
        {
            Hurt(-MaxHp * item.effectsVaule);
        }
        InventoryManager.SaveInventory(item, -1);
    }

}
