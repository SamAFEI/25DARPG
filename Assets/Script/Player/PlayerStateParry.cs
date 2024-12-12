using UnityEngine;

public class PlayerStateParry : PlayerStateGrounded
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
        player.StunnedTrigger(0);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isAnimFinish)
        {
            FSM.SetNextState(player.idleState);
            return;
        }
    }
}
