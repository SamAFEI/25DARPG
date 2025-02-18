﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : Entity
{
    #region Components
    public PlayerInput input { get; set; }
    public UI_PlayerStatus uiPlayerStatus { get; private set; }
    public UI_PlayerHint uiPlayerHint { get; private set; }
    public UI_Interactable uiInteractable { get; private set; }
    public GameObject weaponPoint;
    #endregion

    #region FSM States
    public PlayerStateIdle idleState { get; set; }
    public PlayerStateRun runState { get; set; }
    public PlayerStateSword swordState { get; set; }
    public PlayerStateDash dashState { get; set; }
    public PlayerStateStun stunState { get; set; }
    public PlayerStateParry parryState { get; set; }
    public PlayerStateParrySuscces parrySusccesState { get; set; }
    public PlayerStateHurt hurtState { get; set; }
    public PlayerStateSex sexState { get; set; }
    public PlayerStateSexStart sexStartState { get; set; }
    public PlayerStateDie dieState { get; set; }
    public PlayerStateEarthshatter earthshatterState { get; set; }
    #endregion

    public new PlayerData Data => (PlayerData)base.Data;
    public bool CanMovement
    {
        get
        {
            return !input.IsAttacking && !input.IsDashing && !input.IsParrying
                && !IsStunning && !IsSexing && !IsSystem && !IsHurting && !IsDied;
        }
    }
    public Vector3 MoveInput { get { return input.MoveInput; } }
    public bool IsSystem { get; set; }
    public bool IsBreak1 { get; set; }
    public bool IsBreak2 { get; set; }
    public string sexAnimName { get; set; }
    public string testSexAnim { get; set; }
    public int testSexAnimIndex { get; set; }
    public bool hasEarthshatter { get { return InventoryManager.Instance.inventories.Where(x => x.item.name == "Earthshatter").ToList().Count() > 0; } }
    public int MaxMp { get; set; }
    public int CurrentMp { get; set; }

    public float LastResistTime;
    public float ShakeTime;

    public SpriteLibraryAsset SLAssetNormal;
    public SpriteLibraryAsset SLAssetBreak1;
    public SpriteLibraryAsset SLAssetBreak2;

    protected override void Awake()
    {
        base.Awake();
        MaxHp = Data.MaxHP;
        MaxMp = Data.MaxMP;
        CurrentHp = MaxHp;
        CurrentMp = 0;
        AttackDamage = Data.AttackDamage;
        input = GetComponent<PlayerInput>();
        uiPlayerHint = GetComponentInChildren<UI_PlayerHint>();
        idleState = new PlayerStateIdle(this, FSM, "Idle");
        runState = new PlayerStateRun(this, FSM, "Run");
        swordState = new PlayerStateSword(this, FSM, "Sword1");
        dashState = new PlayerStateDash(this, FSM, "Dash");
        stunState = new PlayerStateStun(this, FSM, "Stun");
        parryState = new PlayerStateParry(this, FSM, "Parry");
        parrySusccesState = new PlayerStateParrySuscces(this, FSM, "ParrySuscces");
        hurtState = new PlayerStateHurt(this, FSM, "Hurt");
        sexState = new PlayerStateSex(this, FSM, "Sex");
        sexStartState = new PlayerStateSexStart(this, FSM, "SexStart");
        dieState = new PlayerStateDie(this, FSM, "Die");
        earthshatterState = new PlayerStateEarthshatter(this, FSM, "Earthshatter");
        FSM.InitState(idleState);
    }

    protected override void Start()
    {
        base.Start();
        uiPlayerStatus = GameObject.FindObjectOfType<UI_PlayerStatus>();
        uiInteractable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
    }

    protected override void Update()
    {
        base.Update();
        LastResistTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            testSexAnimIndex++;
            if (testSexAnimIndex > 2) testSexAnimIndex = 1;
            testSexAnim = "OrcForeplay" + testSexAnimIndex.ToString().PadLeft(2, '0');
            sexAnimName = testSexAnim;
            IsSexing = true;
            Debug.Log(sexAnimName);
            StartCoroutine(TestSexAnimation(sexState));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            testSexAnimIndex++;
            if (testSexAnimIndex > 2) testSexAnimIndex = 1;
            testSexAnim = "OrcSex" + testSexAnimIndex.ToString().PadLeft(2, '0');
            sexAnimName = testSexAnim;
            IsSexing = true;
            Debug.Log(sexAnimName);
            StartCoroutine(TestSexAnimation(sexStartState));
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            FSM.SetNextState(idleState);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Hurt(10000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            entityFX.DoPlayBuffFX(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DoEarthshatter();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Hurt(-10000);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (CanMovement)
        {
            if (MoveInput.x != 0)
            {
                CheckIsFacingRight(MoveInput.x > 0);
            }
            if (MoveInput.x > 0 != IsFacingRight)
            {
                //避免轉向滑行
                SetZeroVelocity();
            }
            Run(1);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            //用攻擊打掉
            if ((other.transform.position - entityCollider.ClosestPoint(other.transform.position)).magnitude > 0.3f) { return; }
            if (IsHurting || IsStunning || IsSuperArmeding) { return; }
            ProjectileBase _projectile = other.GetComponentInParent<ProjectileBase>();
            Repel(_projectile.transform.position);
            Hurt(_projectile.AttackDamage, _projectile.IsHeaveyAttack);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemyAttack")
        {
            Enemy _enemy = other.GetComponentInParent<Enemy>();
            if (CanBeStunned && _enemy.CanBeStunned && !_enemy.IsStunning)
            {
                TimerManager.Instance.DoFrozenTime(0.15f);
                entityFX.DoPlayHitFX(0, weaponPoint.transform.position);
                _enemy.LastStunTime = 3f;
                FSM.SetNextState(parrySusccesState);
                return;
            }
            if (IsHurting || IsStunning || IsSuperArmeding || !_enemy.CanDamage || IsSexing) { return; }
            if (_enemy.IsCatching)
            {
                if (_enemy.IsDied) { return; }
                if (!IsBreak1)
                {
                    sexAnimName = _enemy.Data.foreplayAnims[Random.Range(0, _enemy.Data.foreplayAnims.Count)].name;
                    FSM.SetNextState(sexState);
                }
                else
                {
                    sexAnimName = _enemy.Data.sexAnims[Random.Range(0, _enemy.Data.sexAnims.Count)].name;
                    FSM.SetNextState(sexStartState);
                }
                IsSexing = true;
                GameManager.AddSexEnemies(_enemy);
                return;
            }
            float damage = _enemy.AttackDamage * Random.Range(0.80f, 1.20f);
            Repel(_enemy.transform.position, _enemy.IsHeaveyAttack);
            Hurt(damage, _enemy.IsHeaveyAttack);
        }
        if (other.tag == "Fire" && !IsAttacking)
        {
            Repel(other.transform.position);
            Hurt(20);
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

    public virtual void Hurt(float _damage, bool _isHeaveyAttack = false)
    {
        //DoDamageHp
        if (_damage > 0)
        {
            if (IsHurting || IsStunning || IsSuperArmeding) { return; }
            CameraManager.Shake(3f, 0.1f);
            LastHurtTime = Data.hurtResetTime;
            StartCoroutine(HurtFlasher());
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
        }
        //HealHp
        else
        {
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth(1f);
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
        //CameraManager.Shake(8f, 0.2f);
        foreach (Enemy _enemy in GameManager.Instance.sexEnemies)
        {
            float _damage = _enemy.AttackDamage;
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
            uiPlayerHint.SetSliderValue(-50);
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
        if (input.IsDashing) { return; }
        IsSystem = true;
        Vector3 _vector = CheckRelativeVector(sourcePosition);
        float faceRight = rb.mass * -5;
        if (isHeavyAttack)
        {
            faceRight *= 3;
            CheckIsFacingRight(_vector.x > 0);
        }
        _vector = new Vector3(_vector.x, 0, _vector.z).normalized * faceRight;
        _vector = CameraManager.GetDirectionByCamera(_vector);
        SetZeroVelocity();
        rb.AddForce(_vector, ForceMode.Impulse);
        IsSystem = false;
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
    /// 掙扎Sex
    /// </summary>
    /// <returns></returns>
    public IEnumerator Resist()
    {
        float _currentInput;
        float _nextInput = 0;
        while (IsSexing)
        {
            if (MoveInput.x != 0)
            {
                _currentInput = MoveInput.x;
                if (_currentInput == _nextInput)
                {
                    SetShakeTime();
                    if (uiPlayerHint.SetSliderValue(10))
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

    public void SetShakeTime()
    {
        float time = 0.1f;
        ShakeTime += time;
        if (ShakeTime > time) { return; } //Coroutine 執行中
        StartCoroutine(DoShake());
    }

    public IEnumerator DoShake()
    {
        Vector3 original = transform.position;
        while (ShakeTime > 0)
        {
            ShakeTime -= Time.deltaTime;
            float offset = Mathf.Sin(Time.time * .5f) * .1f;
            transform.position = original + new Vector3(offset, 0, offset);
            yield return new WaitForSeconds(0.001f);
        }
        transform.position = original;
    }

    #region Input Action
    public void ItemAction(int _index)
    {
        Inventory inventory = UI_Shortcut.Instance.GetSlotItemData(_index);
        if (inventory == null || inventory.amount == 0) return;

        ItemData item = inventory.item;
        if (item.name == "HPPotion")
        {
            PlaySFXTrigger(0);
            Hurt(-MaxHp * item.effectsVaule);
        }
        //else if (item.name == "MPPotion")
        //{
        //    PlaySFXTrigger(0);
        //}
        InventoryManager.SaveInventory(item, -1);
    }
    #endregion

    #region Input Skill
    public bool CanEarthshatter()
    {
        return hasEarthshatter;
    }
    #endregion

    public void DoEarthshatter()
    {
        input.SetAttacking(true, AttackTypeEnum.Earthshatter);
    }
    public IEnumerator TestSexAnimation(EntityState sexState)
    {
        FSM.SetNextState(idleState);
        yield return new WaitForSeconds(0.3f);
        FSM.SetNextState(sexState);
    }
    public IEnumerator DemoSword3()
    {
        IsSystem = true;
        input.inputHandle.Character.Disable();
        input.MoveInput = Vector2.zero;
        input.SetAttacking(true, AttackTypeEnum.Earthshatter);
        yield return new WaitForSeconds(1f);
        input.inputHandle.Character.Enable();
        IsSystem = false;
    }
    public IEnumerator DoStartAnimation()
    {
        IsSystem = true;
        input.inputHandle.Character.Disable();
        input.inputHandle.SexAction.Disable();
        while (true)
        {
            float index = Random.Range(1, 5);
            if (index < 3)
            {
                if (index == 1)
                    testSexAnim = "OrcForeplay01";
                else if (index == 2)
                    testSexAnim = "OrcForeplay03";
                SetSpriteLibraryAsset(SLAssetBreak1);
            }
            else
            {
                if (index == 3)
                    testSexAnim = "OrcSex01";
                else if (index == 4)
                    testSexAnim = "OrcSex04";
                SetSpriteLibraryAsset(SLAssetBreak2);
            }
            anim.Play(testSexAnim);
            yield return new WaitForSeconds(5f);
        }
    }

    public virtual void DoAttactMove(float speed)
    {
        if (MoveInput.x != 0)
        {
            CheckIsFacingRight(MoveInput.x > 0);
        }
        Run(speed);
    }

    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        float factor = 1.6f;
        Vector3 targetDirection = CameraManager.GetDirectionByCamera(MoveInput.z, MoveInput.x);
        Vector3 veloctiy = rb.velocity;
        veloctiy.y = 0;
        if (CameraManager.Instance.activeCamera != null)
        {
            if (CameraManager.Instance.isFaceZ)
            {
                veloctiy.z /= factor;
            }
            else
            {
                veloctiy.x /= factor;
            }
        }
        float rbSpeed = veloctiy.magnitude;
        rbSpeed = Mathf.Clamp(rbSpeed, 0, Data.runMaxSpeed);
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = targetDirection.magnitude * Data.runMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        if (lerpAmount > 1) { targetSpeed *= lerpAmount; }
        targetSpeed = Mathf.Lerp(rbSpeed, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.

        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        /*if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(rb.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= data.jumpHangAccelerationMult;
            targetSpeed *= data.jumpHangMaxSpeedMult;
        }*/
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        /*if (data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }*/
        #endregion
        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rbSpeed;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        Vector3 dir = targetDirection;
        if (MoveInput.z != 0 && CameraManager.Instance.activeCamera != null)
        {
            if (CameraManager.Instance.isFaceZ)
            {
                dir.z = MoveInput.z > 0 ? factor : -factor;
            }
            else
            {
                dir.x = MoveInput.z > 0 ? -factor : factor;
            }
        }

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * dir, ForceMode.Force);
        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
    }
    #endregion
}
