using UnityEngine;

public class PlayerStateStun : PlayerState
{
    public PlayerStateStun(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
        stateTime = 3f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log(stateTime);
        if (!player.input.IsStunning)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
        if (stateTime < 0)
        {
            FSM.ChangeState(player.hToEnemyState);
            return;
        }
    }
}
