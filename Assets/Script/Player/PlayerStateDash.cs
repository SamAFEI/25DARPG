using UnityEngine;

public class PlayerStateDash : PlayerState
{
    public PlayerStateDash(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
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
