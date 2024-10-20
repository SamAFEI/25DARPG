using UnityEngine;

public class PlayerStateSex : PlayerState
{
    public PlayerStateSex(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        animName = player.sexAnimName;
        base.OnEnter();
        player.input.inputHandle.Character.Disable();
        player.input.inputHandle.SexAction.Enable();
        player.uiPlayerHint.SetResistHint(SexResistEnum.Horizontal, true);
        player.IsSexing = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        player.input.inputHandle.Character.Enable();
        player.input.inputHandle.SexAction.Disable();
        GameManager.ResetSexEnemies();
        player.uiPlayerHint.SetResistHint(SexResistEnum.Horizontal, false);
        player.SetBreak();
        player.IsSexing = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        player.SetZeroVelocity();
        if (player.IsDied)
        {
            FSM.ChangeState(player.dieState);
            return;
        }
        if (!player.IsSexing)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
