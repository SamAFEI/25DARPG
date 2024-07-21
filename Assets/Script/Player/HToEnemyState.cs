using UnityEngine;

public class HToEnemyState : PlayerState
{
    public HToEnemyState(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!player.input.IsStunning)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
