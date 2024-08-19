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
        player.hurtCount = 0;
        stateTime = 3f;
        player.LastStunTime = stateTime;
    }

    public override void OnExit()
    {
        base.OnExit();
        player.LastStunTime = 0;
    }

    public override void OnUpdate()
    {
        player.SearchSexEnemy();
        base.OnUpdate();
        if (!player.IsStunning)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
        if (player.IsSexing)
        {
            FSM.ChangeState(player.sexState);
            return;
        }
    }
}
