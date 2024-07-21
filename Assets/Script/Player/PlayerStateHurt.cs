public class PlayerStateHurt : PlayerState
{
    public PlayerStateHurt(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
    }

    public override void OnExit()
    {
        base.OnExit();
        player.input.SetHurting(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate(); 
        if (isAnimFinish)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
