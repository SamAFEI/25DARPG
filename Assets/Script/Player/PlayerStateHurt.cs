using UnityEngine;

public class PlayerStateHurt : PlayerState
{
    public PlayerStateHurt(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.LastSuperArmedTime = player.Data.hurtResetTime;
    }

    public override void OnExit()
    {
        base.OnExit();
        player.LastHurtTime = 0f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (player.CurrentHp <= 0)
        {
            FSM.SetNextState(player.dieState);
            return;
        }
        if (player.IsStunning)
        {
            FSM.SetNextState(player.stunState);
            return;
        }
        if (isAnimFinish)
        {
            FSM.SetNextState(player.idleState);
            return;
        }
    }
}
