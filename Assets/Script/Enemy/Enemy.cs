using UnityEngine;

public class Enemy : Entity
{
    public UI_EntityStatus uiEnityStatus { get; private set; }

    #region FSM States
    public EnemyStateIdle idleState { get; set; }
    public EnemyStateRun runState { get; set; }
    public EnemyStateSword swordState { get; set; }
    public EnemyStateHurt hurtState { get; set; }
    public EnemyStateAlert alertState { get; set; }
    public EnemyStateChase chaseState { get; set; }
    //public EnemyStateDash dashState { get; set; }
    //public EnemyStateStun stunState { get; set; }
    #endregion
    public new EnemyData Data => (EnemyData)base.Data;

    #region Timers
    public float LastAlertTime;
    public float LastAttackTime;
    #endregion

    public Vector3 MoveTarget;
    public bool IsAlerting;
    public bool IsAttacking;
    public bool CanChase;
    public bool CanAttack;
    public bool CanSexPlayer;

    public float Attack1Distance;

    protected override void Awake()
    {
        base.Awake();
        MaxHp = 100;
        CurrentHp = MaxHp;
        AttackDamage = 10;
        Attack1Distance = 2f;
        IsAlerting = true;
        idleState = new EnemyStateIdle(this, FSM, "Idle");
        runState = new EnemyStateRun(this, FSM, "Run");
        swordState = new EnemyStateSword(this, FSM, "Sword1");
        hurtState = new EnemyStateHurt(this, FSM, "Hurt");
        alertState = new EnemyStateAlert(this, FSM, "Alert");
        chaseState = new EnemyStateChase(this, FSM, "Run");
        FSM.InitState(idleState);
        sexAnimName = "Sex01";
    }

    protected override void Start()
    {
        base.Start();
        uiEnityStatus = GetComponentInChildren<UI_EntityStatus>();
    }

    protected override void Update()
    {
        base.Update();

        #region Timers
        LastAlertTime -= Time.deltaTime;
        LastAttackTime -= Time.deltaTime;
        #endregion
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        DoAlert();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack")
        {
            Player _player = other.GetComponentInParent<Player>();
            float power = 10;
            float faceDir = 1;
            float DirZ = 0;
            if (_player.IsHeaveyAttack) { power *= power; }
            if (_player.transform.position.x > transform.position.x)
            {
                faceDir *= -1;
            }
            if (_player.transform.position.z > transform.position.z) { DirZ = -10; }
            if (_player.transform.position.z < transform.position.z) { DirZ = 10; }
            CheckIsFacingRight(faceDir*-1 > 0);
            entityFX.DoPlayHitFX(0, skeleton.transform.position);
            rb.AddForce(new Vector3(faceDir * power, 0, DirZ), ForceMode.Impulse);
            Hurt(_player.AttackDamage, _player.IsHeaveyAttack);
        }
    }

    public virtual void Hurt(float _damage, bool _isHeaveyAttack = false)
    {
        if (_damage > 0)
        {
            if (IsHurting) { return; }
            TimerManager.Instance.DoFrozenTime(0.1f);
            CameraManager.Instance.Shake(1f, 0.1f);
            LastHurtTime = Data.hurtResetTime;
            if (_isHeaveyAttack) { LastHurtTime += 0.3f; }
            StartCoroutine(HurtFlasher());
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiEnityStatus.DoLerpHealth();
        }
        else
        {
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiEnityStatus.DoLerpHealth();
        }
    }

    public virtual void DoAlert()
    {
        if (!IsAlerting || IsAttacking) 
        {
            CanChase = false;
            CanAttack = false;
            return; 
        }
        Vector3 _vector = GameManager.GetPlayerDirection(this.transform.position);
        if (_vector.x != 0)
        {
            CheckIsFacingRight(_vector.x > 0);
        }

        CanChase = LastAttackTime < 0 && GameManager.GetPlayerDistance(this.transform.position) > Attack1Distance;
        CanAttack = LastAttackTime < 0 && GameManager.GetPlayerDistance(this.transform.position) < Attack1Distance 
            && GameManager.CanAttackPlayer();
        CanSexPlayer = GameManager.CanSexPlayer() && GameManager.GetPlayerDistance(this.transform.position) < Attack1Distance;
    }

    public virtual void DoChase()
    {
        if (IsHurting && IsAttacking) { return ; }
        if (GameManager.GetPlayerDistance(this.transform.position) > Attack1Distance)
        {
            MoveTarget = GameManager.GetPlayerDirection(this.transform.position);
            Run(1);
        }
    }

    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        float rbSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = MoveTarget.normalized.magnitude * Data.runMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
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
        rb.AddForce(movement * MoveTarget.normalized, ForceMode.Force);
        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
    }
    #endregion
}

