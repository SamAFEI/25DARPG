using UnityEngine;

public class PlayerStateDash : PlayerState
{
    public PlayerStateDash(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
        player.IgnoreLayersTrigger(1);
        player.input.ResetDashTime = 0.6f;
    }
    public override void OnExit()
    {
        base.OnExit();
        player.IgnoreLayersTrigger(0);
        player.SetZeroVelocity();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!player.input.IsDashing)
        {
            FSM.SetNextState(player.idleState);
            return;
        }
    }
}
