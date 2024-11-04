using UnityEngine;

public class PlayerStateIdle : PlayerStateGrounded
{
    public PlayerStateIdle(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (player.CanMovement && (player.MoveInput.x != 0 || player.MoveInput.z != 0))
        {
            FSM.SetNextState(player.runState);
            return;
        }
    }
}
