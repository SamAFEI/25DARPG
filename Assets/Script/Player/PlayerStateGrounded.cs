using UnityEngine;

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
        
        if (player.IsDied)
        {
            FSM.SetNextState(player.dieState);
            return;
        }
        if (player.IsHurting && !player.IsSuperArmeding)
        {
            FSM.SetNextState(player.hurtState);
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
            FSM.SetNextState(player.dashState);
            return;
        }
        if (player.input.IsParrying && FSM.currentState != player.parrySusccesState)
        {
            FSM.SetNextState(player.parryState);
            return;
        }
        if (player.input.IsAttacking)
        {
            if (player.AttackType == AttackTypeEnum.Basic)
            {
                FSM.SetNextState(player.swordState);
                return;
            }
            else if (player.AttackType == AttackTypeEnum.Earthshatter)
            {
                FSM.SetNextState(player.earthshatterState);
                return;
            }
        }
    }
}
