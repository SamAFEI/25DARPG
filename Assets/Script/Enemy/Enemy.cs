using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public UI_EntityStatus uiEnityStatus { get; private set; }
    public UI_BossStatus uiBossStatus { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }

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
    public EnemyStatePatrolMove patrolMoveState { get; set; }
    public EnemyStatePatrolIdle patrolIdleState { get; set; }
    public EnemyStateDash dashState { get; set; }
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
    public bool IsPatroling { get; set; }
    public bool IsKeepawaying { get; set; }
    public bool IsCatching { get; set; }
    public bool IsAmbushDash { get; set; }
    public bool CanChase { get; set; }
    public bool CanAttack1 { get; set; }
    public bool CanAttack2 { get; set; }
    public bool CanAttack3 { get; set; }
    public bool CanCatch { get; set; }
    public bool IsNoCDAttack { get; set; }
    public float ChaseSpeed { get; set; }
    public Vector3 MoveTarget;
    public Vector3 MoveDirection;
    public PatrolData patrolData = new PatrolData();


    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        MaxHp = Data.maxHP;
        CurrentHp = MaxHp;
        AttackDamage = 10;
        AttackMoveMaxSpeed = 3f;
        ChaseSpeed = 1f;
        patrolData.Index = 0;
        patrolData.Order = 1;
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
            uiEnityStatus.GetComponent<RectTransform>().localScale = transform.localScale * 0.01f;
        }
        LastCatchTime = Random.Range(3.0f, 5.0f);
        if (patrolData.Points.Count > 0)
        {
            FSM.SetNextState(patrolMoveState);
        }
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
        if (IsAlerting)
        {
            GameManager.AddAlertEnemy(this);
        }
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
            PlayHurtSFXTrigger();
            Hurt(_player.AttackDamage, _player.IsHeaveyAttack);
            Repel(_player.transform.position, _player.IsHeaveyAttack);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.tag == "Fire")
        {
            Repel(other.transform.position);
            Hurt(20);
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
        patrolIdleState = new EnemyStatePatrolIdle(this, FSM, "Alert");
        patrolMoveState = new EnemyStatePatrolMove(this, FSM, "Run");
        dashState = new EnemyStateDash(this, FSM, "Dash");
        FSM.InitState(idleState);
    }
    public float GetPlayerDistance() => GameManager.GetPlayerDistance(this.transform.position);
    public Vector3 GetPlayerDirection() => GameManager.GetPlayerDirection(this.transform.position);

    #region About Hurt
    public virtual void Hurt(float _damage, bool _isHeaveyAttack = false)
    {
        if (_damage > 0)
        {
            if (IsHurting) { return; }
            TimerManager.Instance.DoFrozenTime(0.1f);
            CameraManager.Shake(1f, 0.1f);
            LastHurtTime = Data.hurtResetTime;
            if (_isHeaveyAttack) { LastHurtTime += 0.3f; }
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
        if (isHeavyAttack) { power *= 1.5f; }
        Vector3 _vector = CheckRelativeVector(sourcePosition);
        float faceRight = _vector.x * -1;
        float faceFoward = _vector.z * -10;
        CheckIsFacingRight(faceRight * -1 > 0);
        _vector = new Vector3(faceRight * power, 0, faceFoward);
        _vector = CameraManager.GetDirectionByCamera(_vector);
        SetZeroVelocity();
        rb.AddForce(_vector, ForceMode.Impulse);
    }
    #endregion

    public override void Die(float _delay = 0.5F)
    {
        base.Die(_delay);
        GameManager.RemoveAlertEnemy(this);
    }

    public override void SetZeroVelocity()
    {
        base.SetZeroVelocity();
        if (navMeshAgent != null)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
    }

    public override void ShotProjectile(int _index)
    {
        GameObject obj = Instantiate(Data.projectiles[_index], attackMesh.transform.position, Quaternion.identity);
        ProjectileBase projectile = obj.GetComponent<ProjectileBase>();
        projectile.AttackDamage = AttackDamage;
        obj.transform.LookAt(GameManager.Instance.player.transform);
        obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * projectile.Speed;
    }

    #region Enemy AI
    public virtual bool CheckPlayerDistance(float minBorder, bool checkZ = true)
    {
        Vector3 vector = CheckRelativeVector(GameManager.Instance.player.transform.position);
        float minZ = attackMesh.GetComponent<BoxCollider>().size.z * 0.5f;
        if (GetPlayerDistance() < minBorder && (Mathf.Abs(vector.z) < minZ || !checkZ))
        {
            return true;
        }
        return false;
    }
    public virtual void CheckAlert()
    {
        if (GameManager.GetPlayerDistance(this.transform.position) <= Data.alertDistance)
        {
            IsAlerting = true;
        }
    }
    public virtual void DoAlert()
    {
        if (!IsAlerting) { return; }
        //if (IsAlerting || IsAttacking)
        //{
        //    CanChase = false;
        //    CanAttack1 = false;
        //    CanCatch = false;
        //    IsKeepawaying = false;
        //    return;
        //}
        float faceRight = CheckRelativeVector(GameManager.Instance.player.transform.position).x;
        if (faceRight != 0 && !IsStunning && !IsCatching)
        {
            CheckIsFacingRight(faceRight > 0);
        }
        CheckAction();
    }
    public virtual void CheckAction()
    {
        IsKeepawaying = (!GameManager.CanAttackPlayer() || LastAttack1Time > 0) && CheckPlayerDistance(Data.alertDistance * 1.2f, false);
        CanChase = LastAttack1Time < 0 && !CheckPlayerDistance(Data.attack1Distance)
            && GameManager.CanAttackPlayer();
        CanAttack1 = LastAttack1Time < 0 && CheckPlayerDistance(Data.attack1Distance)
            && GameManager.CanAttackPlayer();
        CanAttack2 = LastAttack2Time < 0 && CheckPlayerDistance(Data.attack2Distance)
            && GameManager.CanAttackPlayer();
        CanAttack3 = LastAttack3Time < 0 && CheckPlayerDistance(Data.attack3Distance)
            && GameManager.CanAttackPlayer();
        CanCatch = LastCatchTime < 0 && CheckPlayerDistance(Data.catchDistance)
            && GameManager.CanAttackPlayer();

        if (GameManager.Instance.player.IsSexing)
        {
            LastAttack1Time = Random.Range(0.3f ,1f);
            LastAttack2Time = Random.Range(0.3f, 1f);
            LastAttack3Time = Random.Range(0.3f, 1f);
            LastCatchTime = Random.Range(3f, 5f);
        }
        if (IsSexing) //觸發Sex的不要動
        { IsKeepawaying = false; }
    }
    public virtual void DoChase()
    {
        if (IsHurting && IsAttacking) { return; }
        //float distance = GetPlayerDistance();
        //if (distance > Data.attack1Distance)
        {
            SetAttackMoveDirection();
            Run(ChaseSpeed);
        }
    }
    public virtual void Keepaway()
    {
        if (IsHurting && IsAttacking) { return; }
        Vector3 _vector = GameManager.GetPlayerDirection(this.transform.position);
        MoveDirection = new Vector3(_vector.x * -1.5f, _vector.y, _vector.z * -1f);
        Run(0.6f);
    }
    public virtual void AlertStateAction()
    {
        if (IsAmbushDash)
        {
            FSM.SetNextState(dashState);
            IsAmbushDash = false;
            return;
        }
        if (CanCatch && Random.Range(0.00f, 100.00f) < 80f)
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
            FacingToPlayer();
            FSM.SetNextState(attack1State);
            return;
        }
        if (CanChase)
        {
            ChaseSpeed = 1;
            if (Random.Range(0,100) > 80)
            { ChaseSpeed = Data.dashSpeed; }
            FSM.SetNextState(chaseState);
            return;
        }
    }
    public virtual void SetMoveDirection(Vector3 start)
    {
        MoveTarget = GameManager.Instance.playerObj.transform.position;
        MoveDirection = GameManager.GetPlayerDirection(start);
    }
    public virtual void SetMoveDirection(Vector3 start, Vector3 target)
    {
        MoveTarget = target;
        MoveDirection = (target - start);
        MoveDirection.y = 0;
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
    public virtual void DoPatrol()
    {
        if (IsAlerting || patrolData.Points.Count == 0) { return; }
        SetMoveDirection(transform.position, patrolData.Points[patrolData.Index].transform.position);
        Run(0.6f);
    }
    public bool ArriveaPatrolPoing()
    {
        if (patrolData.NextPoint == Vector3.zero) 
        { 
            patrolData.NextPoint = patrolData.Points[0].position; 
        }
        float dis = Vector3.Distance(patrolData.NextPoint, transform.position);
        if (dis < 3f)
        {
            patrolData.Index += patrolData.Order;
            if (patrolData.Index >= patrolData.Points.Count || patrolData.Index <= -1)
            {
                patrolData.Order *= -1;
                patrolData.Index += patrolData.Order;
            }
            patrolData.NextPoint = patrolData.Points[patrolData.Index].position;
            patrolData.NextPoint.x += Random.Range(-2f, 2f);
            patrolData.NextPoint.z += Random.Range(-2f, 2f);
            return true;
        }
        return false;
    }
    public virtual IEnumerator DoBreakAndDash()
    {
        FacingToPlayer();
        IsNoCDAttack = true;
        FSM.SetNextState(attack1State);
        yield return new WaitForSeconds(0.2f);
        CanChase = true; //避開 ObstacleAvoidance 用
        FSM.SetNextState(dashState);
    }
    public virtual void DashAttack()
    {
        if (Random.Range(0, 100) > 40)
        {
            FSM.SetNextState(attack1State);
            return;
        }
        if (!Data.isBoos) //Boss沒有小動畫
        {
            FSM.SetNextState(catchState);
        }
    }
    public virtual bool CheckDashAttack()
    {
        return CheckPlayerDistance(Data.attack1Distance)
            && GameManager.CanAttackPlayer();
    }
    #endregion

    #region RUN METHODS
    public void Run(float lerpAmount)
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

        if (ObstacleAvoidance()) { return; }

        navMeshAgent.isStopped = true; //避免NavMeshAgent干擾AddForce
        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * MoveDirection.normalized * rb.mass / 10f, ForceMode.Force);

        float faceRight = CheckRelativeVector(transform.position + MoveDirection).x;
        if (faceRight != 0)
        {
            CheckIsFacingRight(faceRight > 0);
        }
        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
    }
    public bool ObstacleAvoidance()
    {
        Vector3 pos = new Vector3(transform.position.x, 0.5f, transform.position.z);
        List<Vector3> directions = new List<Vector3>()
        {
            MoveDirection.normalized,
            Quaternion.AngleAxis(30f, Vector3.up) * MoveDirection.normalized,
            Quaternion.AngleAxis(-30f, Vector3.up) * MoveDirection.normalized,
        };
        foreach (Vector3 direction in directions)
        {
            DrawRay(pos, direction);
            RaycastHit hit;
            if (Physics.Raycast(pos, direction, out hit, 2f))
            {
                if (CanChase && hit.transform.tag == "Destructible")
                {
                    DestructibleEntity destructible = hit.transform.GetComponentInParent<DestructibleEntity>();
                    if (destructible != null)
                    {
                        if (destructible.hp == 0) { return false; }
                        if (destructible.hp <= Data.attack1Damage && LastAttack1Time < 0 
                            && destructible.destructibleType == DestructibleType.wood && Random.Range(0, 100) > 70)
                        {
                            FSM.SetNextState(attack1State);
                        }
                    }
                }
                navMeshAgent.SetDestination(MoveTarget);
                navMeshAgent.isStopped = false;
                return true;
            }
        }
        return false;
    }
    #endregion

    private void DrawRay(Vector3 pos, Vector3 rayDirections)
    {
        Debug.DrawRay(pos, rayDirections.normalized * 2f, Color.blue);
    }
}

