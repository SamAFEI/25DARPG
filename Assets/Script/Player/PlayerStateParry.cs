public class PlayerStateParry : PlayerState
{
    public PlayerStateParry(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
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
        player.input.SetParrying(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!player.input.CanCounter && false)
        {
            //FSM.ChangeState(player.counterState);
            return;
        }
        if (!player.input.IsParrying)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
