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
    }
    public override void OnExit()
    {
        base.OnExit();
        player.IgnoreLayersTrigger(0);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!player.input.IsDashing)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
