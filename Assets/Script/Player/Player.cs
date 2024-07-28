using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : Entity
{
    public PlayerInput input { get; set; }
    public UI_PlayerStatus uiPlayerStatus { get; private set; }

    #region FSM States
    public PlayerStateIdle idleState { get; set; }
    public PlayerStateRun runState { get; set; }
    public PlayerStateSword swordState { get; set; }
    public PlayerStateDash dashState { get; set; }
    public PlayerStateStun stunState { get; set; }
    public PlayerStateParry parryState { get; set; }
    public PlayerStateHurt hurtState { get; set; }
    public HToEnemyState hToEnemyState { get; set; }
    #endregion
    public new PlayerData Data => (PlayerData)base.Data;
    public Vector3 MoveInput { get { return input.MoveInput; } }
    public SpriteLibraryAsset SLAssetNormal;
    public SpriteLibraryAsset SLAssetBreak1;
    public SpriteLibraryAsset SLAssetBreak2;

    protected override void Awake()
    {
        MaxHp = 100;
        CurrentHp = MaxHp;
        AttackDamage = 10;
        base.Awake();
        input = GetComponent<PlayerInput>();
        idleState = new PlayerStateIdle(this, FSM, "Idle");
        runState = new PlayerStateRun(this, FSM, "Run");
        swordState = new PlayerStateSword(this, FSM, "Sword1");
        dashState = new PlayerStateDash(this, FSM, "Dash");
        stunState = new PlayerStateStun(this, FSM, "Stun");
        parryState = new PlayerStateParry(this, FSM, "Parry");
        hurtState = new PlayerStateHurt(this, FSM, "Hurt");

        hToEnemyState = new HToEnemyState(this, FSM, "HToPuppet");
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

    #region HurtFlash
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
    #endregion

    public virtual void Hurt(float _damage)
    {
        if (IsHurting || IsStunning || IsSuperArmeding) { return; }
        CameraManager.Instance.Shake(3f, 0.1f);
        LastHurtTime = Data.hurtResetTime;
        StartCoroutine(HurtFlasher());
        CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
        uiPlayerStatus.DoLerpHealth(_damage);
    }
}
