using UnityEngine;

public class PlayerStateParrySuscces : PlayerStateGrounded
{
    public PlayerStateParrySuscces(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
        player.LastSuperArmedTime = 10f;
    }

    public override void OnExit()
    {
        base.OnExit();
        player.input.SetParrying(false);
        player.LastSuperArmedTime = 0f;
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
