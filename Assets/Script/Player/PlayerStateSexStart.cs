using UnityEngine;

public class PlayerStateSexStart : PlayerState
{
    public PlayerStateSexStart(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        animName = player.sexAnimName + "Start";
        base.OnEnter();
        player.input.inputHandle.Character.Disable();
        player.IsSexing = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        player.input.inputHandle.Character.Enable();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        player.SetZeroVelocity();
        if (isAnimFinish)
        {
            FSM.SetNextState(player.sexState);
            return;
        }
    }
}
