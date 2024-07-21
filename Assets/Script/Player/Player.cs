using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : Entity
{
    public PlayerInput input { get; set; }
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

    public Vector3 MoveInput { get { return input.MoveInput; } }
    public SpriteLibraryAsset SLAssetNormal;
    public SpriteLibraryAsset SLAssetBreak1;
    public SpriteLibraryAsset SLAssetBreak2;

    protected override void Awake()
    {
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

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
