public class PlayerStateGrounded : PlayerState
{
    public PlayerStateGrounded(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate(); 

        if (player.IsHurting && !player.IsSuperArmeding)
        {
            FSM.ChangeState(player.hurtState);
            return;
        }
        /* 由 Hurt 進入 Stun
        if (player.IsStunning)
        {
            FSM.ChangeState(player.stunState);
            return;
        }*/
        if (player.input.IsDashing)
        {
            FSM.ChangeState(player.dashState);
            return;
        }
        if (player.input.IsParrying)
        {
            FSM.ChangeState(player.parryState);
            return;
        }
        if (player.input.IsAttacking && FSM.currentState != player.swordState)
        {
            FSM.ChangeState(player.swordState);
            return;
        }
    }
}
