using UnityEngine;

public class Enemy : Entity
{
    public UI_EntityStatus uiEnityStatus { get; private set; }
    public UI_BossStatus uiBossStatus { get; private set; }

    #region FSM States
    public EnemyStateIdle idleState { get; set; }
    public EnemyStateRun runState { get; set; }
    public EnemyStateAttack1 attack1State { get; set; }
    public EnemyStateAttack2 attack2State { get; set; }
    public EnemyStateAttack3 attack3State { get; set; }
    public EnemyStateHurt hurtState { get; set; }
    public EnemyStateAlert alertState { get; set; }
    public EnemyStateChase chaseState { get; set; }
    public EnemyStateKeepaway keepawayState { get; set; }
    public EnemyStateCatch catchState { get; set; }
    public EnemyStateDie dieState { get; set; }
    public EnemyStateStun stunState { get; set; }
    public EnemyStateBeCountered beCounteredState { get; set; }
    //public EnemyStateDash dashState { get; set; }
    #endregion
    public new EnemyData Data => (EnemyData)base.Data;

    #region Timers
    public float LastAlertIdleTime { get; set; }
    public float LastAttack1Time { get; set; }
    public float LastAttack2Time { get; set; }
    public float LastAttack3Time { get; set; }
    public float LastCatchTime { get; set; }
    #endregion

    public float AttackMoveMaxSpeed { get; set; }
    public bool IsAlerting { get; set; }
    public bool IsKeepawaying { get; set; }
    public bool IsCatching { get; set; }
    public bool CanChase { get; set; }
    public bool CanAttack1 { get; set; }
    public bool CanAttack2 { get; set; }
    public bool CanAttack3 { get; set; }
    public bool CanSexPlayer { get; set; }
    public bool CanCatch { get; set; }
    public Vector3 MoveDirection;

    public AudioClip sfxAttack;
    public AudioClip sfxAttack2;
    public AudioClip sfxAttack3;
    public AudioClip sfxHurt;

    protected override void Awake()
    {
        base.Awake();
        MaxHp = Data.maxHP;
        CurrentHp = MaxHp;
        AttackDamage = 10;
        AttackMoveMaxSpeed = 3f;
        SetFSMState();
    }

    protected override void Start()
    {
        base.Start();
        if (Data.isBoos)
        {
            uiBossStatus = GameObject.FindAnyObjectByType<UI_BossStatus>();
        }
        else
        {
            uiEnityStatus = GetComponentInChildren<UI_EntityStatus>();
        }
        LastCatchTime = Random.Range(3.0f, 5.0f);
    }

    protected override void Update()
    {
        base.Update();

        #region Timers
        LastAlertIdleTime -= Time.deltaTime;
        LastAttack1Time -= Time.deltaTime;
        LastAttack2Time -= Time.deltaTime;
        LastAttack3Time -= Time.deltaTime;
        LastCatchTime -= Time.deltaTime;
        # endregion
    }

    protected override void FixedUpdate()
    {
        if (IsDied) { return; }
        base.FixedUpdate();
        CheckAlert();
        DoAlert();
        if (IsMoveToTarget && !IsStunning)
        {
            DoMoveTarget();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack")
        {
            Player _player = other.GetComponentInParent<Player>();
            entityFX.DoPlayHitFX(0, entityCollider.ClosestPoint(other.transform.position));
            PlaySFXTrigger(3);
            Hurt(_player.AttackDamage, _player.IsHeaveyAttack);
            Repel(_player.transform.position, _player.IsHeaveyAttack);
        }
    }

    protected virtual void SetFSMState()
    {
        idleState = new EnemyStateIdle(this, FSM, "Idle");
        runState = new EnemyStateRun(this, FSM, "Run");
        attack1State = new EnemyStateAttack1(this, FSM, "Attack1");
        attack2State = new EnemyStateAttack2(this, FSM, "Attack2");
        attack3State = new EnemyStateAttack3(this, FSM, "Attack3");
        hurtState = new EnemyStateHurt(this, FSM, "Hurt");
        alertState = new EnemyStateAlert(this, FSM, "Alert");
        chaseState = new EnemyStateChase(this, FSM, "Run");
        keepawayState = new EnemyStateKeepaway(this, FSM, "Keepaway");
        catchState = new EnemyStateCatch(this, FSM, "Catch");
        dieState = new EnemyStateDie(this, FSM, "Die");
        stunState = new EnemyStateStun(this, FSM, "Stun");
        beCounteredState = new EnemyStateBeCountered(this, FSM, "BeCountered");
        FSM.InitState(idleState);
    }
    public float GetPlayerDistance() => GameManager.GetPlayerDistance(this.transform.position);
    public Vector3 GetPlayerDirection() => GameManager.GetPlayerDirection(this.transform.position);

    #region Animation Action
    public override void PlaySFXTrigger(int _value)
    {
        AudioClip clip = null;
        if (_value == 0) { clip = sfxAttack; }
        else if (_value == 1) { clip = sfxAttack2; }
        else if (_value == 2) { clip = sfxAttack3; }
        else if (_value == 3) { clip = sfxHurt; }
        if (clip != null)
        {
            AudioManager.PlayOnPoint(AudioManager.SFXSource, clip, transform.position);
        }
    }

    #endregion

    #region About Hurt
    public virtual void Hurt(float _damage, bool _isHeaveyAttack = false)
    {
        if (_damage > 0)
        {
            if (IsHurting) { return; }
            TimerManager.Instance.DoFrozenTime(0.1f);
            CameraManager.Shake(1f, 0.1f);
            LastHurtTime = Data.hurtResetTime;
            if (_isHeaveyAttack) { LastHurtTime += 0.3f; _damage *= 2; }
            if (IsSuperArmeding) { LastHurtTime = 0; }
            StartCoroutine(HurtFlasher());
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            if (Data.isBoos) { uiBossStatus.DoLerpHealth(); }
            else { uiEnityStatus.DoLerpHealth(); }
        }
        else
        {
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            if (Data.isBoos) { uiBossStatus.DoLerpHealth(); }
            else { uiEnityStatus.DoLerpHealth(); }
        }

        if (IsDied)
        {
            FSM.SetNextState(dieState);
        }
    }
    public void Repel(Vector3 sourcePosition, bool isHeavyAttack = false)
    {
        if (IsSuperArmeding) return;
        float power = rb.mass * 1.5f;
        if (isHeavyAttack) { power *= 3f; }
        Vector3 _vector = CheckRelativeVector(sourcePosition);
        float faceRight = _vector.x * -1;
        float faceFoward = _vector.z * -10;
        CheckIsFacingRight(faceRight * -1 > 0);
        _vector = new Vector3(faceRight * power, 0, faceFoward);
        _vector = CameraManager.GetDirectionByCamera(_vector);
        rb.AddForce(_vector, ForceMode.Impulse);
    }
    #endregion

    public override void ShotProjectile(int _index)
    {
        GameObject obj = Instantiate(Data.projectiles[_index], attackMesh.transform.position, Quaternion.identity);
        ProjectileBase projectile = obj.GetComponent<ProjectileBase>();
        projectile.AttackDamage = AttackDamage;
        obj.transform.LookAt(GameManager.Instance.player.transform);
        obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * projectile.Speed;
    }
    #region Enemy AI
    public virtual void CheckAlert()
    {
        if (GameManager.GetPlayerDistance(this.transform.position) <= Data.alertDistance)
        {
            IsAlerting = true;
        }
    }
    public virtual void DoAlert()
    {
        if (!IsAlerting || IsAttacking)
        {
            CanChase = false;
            CanAttack1 = false;
            CanCatch = false;
            IsKeepawaying = false;
            return;
        }
        float faceRight = CheckRelativeVector(GameManager.Instance.player.transform.position).x;
        if (faceRight != 0 && !IsStunning)
        {
            CheckIsFacingRight(faceRight > 0);
        }
        CheckAction();
    }
    public virtual void CheckAction()
    {
        IsKeepawaying = LastAttack1Time > 0 && GetPlayerDistance() < Data.alertDistance;
        CanChase = LastAttack1Time < 0 && GetPlayerDistance() > Data.attack1Distance;
        CanAttack1 = LastAttack1Time < 0 && GetPlayerDistance() < Data.attack1Distance
            && GameManager.CanAttackPlayer();
        CanAttack2 = LastAttack2Time < 0 && GetPlayerDistance() < Data.attack2Distance
            && GameManager.CanAttackPlayer();
        CanAttack3 = LastAttack3Time < 0 && GetPlayerDistance() < Data.attack3Distance
            && GameManager.CanAttackPlayer();
        CanCatch = LastCatchTime < 0 && GetPlayerDistance() < Data.catchDistance
            && GameManager.CanAttackPlayer();
        CanSexPlayer = GameManager.CanSexPlayer() && GetPlayerDistance() < Data.catchDistance;
    }
    public virtual void DoChase()
    {
        if (IsHurting && IsAttacking) { return; }
        if (GetPlayerDistance() > Data.attack1Distance)
        {
            SetAttackMoveDirection();
            Run(1);
        }
    }
    public virtual void Keepaway()
    {
        if (IsHurting && IsAttacking) { return; }
        Vector3 _vector = GameManager.GetPlayerDirection(this.transform.position);
        MoveDirection = new Vector3(_vector.x * -1, _vector.y, _vector.z * -1);
        Run(0.6f);
    }
    public virtual void AlertStateAction()
    {
        if (CanCatch && Random.Range(0.00f, 100.00f) < 100f)
        {
            FSM.SetNextState(catchState);
            return;
        }
        if (IsKeepawaying)
        {
            FSM.SetNextState(keepawayState);
            return;
        }
        if (CanAttack1)
        {
            FSM.SetNextState(attack1State);
            CheckIsFacingRight(CheckRelativeVector(GameManager.Instance.player.transform.position).x > 0);
            return;
        }
        if (CanChase)
        {
            FSM.SetNextState(chaseState);
            return;
        }
    }
    public virtual void SetMoveDirection(Vector3 target)
    {
        MoveDirection = GameManager.GetPlayerDirection(target);
    }
    public override void SetAttackMoveDirection()
    {
        SetMoveDirection(attackMesh.transform.position);
    }
    public virtual void DoMoveTarget()
    {
        float faceRight = CheckRelativeVector(MoveDirection + transform.position).x;
        if (faceRight != 0)
        {
            CheckIsFacingRight(faceRight > 0);
        }
        float Speed = MoveDirection.magnitude * 0.6f;
        Speed = Mathf.Clamp(Speed, 1f, AttackMoveMaxSpeed);
        Run(Speed);
    }
    public virtual void Attack1Finish()
    {
        FSM.SetNextState(alertState);
    }
    public virtual void Attack2Finish()
    {
        FSM.SetNextState(alertState);
    }
    public virtual void Attack3Finish()
    {
        FSM.SetNextState(alertState);
    }
    #endregion

    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        float rbSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = MoveDirection.normalized.magnitude * Data.runMaxSpeed;
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

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * MoveDirection.normalized * rb.mass / 10f, ForceMode.Force);
        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
    }
    #endregion
}

