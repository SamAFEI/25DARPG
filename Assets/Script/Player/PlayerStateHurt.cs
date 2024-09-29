public class PlayerStateHurt : PlayerState
{
    public PlayerStateHurt(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
        player.LastSuperArmedTime = player.Data.hurtResetTime;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate(); 
        if (player.CurrentHp <= 0)
        {
            FSM.ChangeState(player.dieState);
            return;
        }
        if (player.IsStunning)
        {
            FSM.ChangeState(player.stunState);
            return;
        }
        if (isAnimFinish)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
