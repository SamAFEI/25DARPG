public class PlayerStateDie : PlayerState
{
    bool isTrigger;
    public PlayerStateDie(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
        isTrigger = false;
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        player.SetZeroVelocity();
        if (isAnimFinish && !isTrigger)
        {
            isTrigger = true;
            player.Die();
        }
        if (!player.IsDied)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
