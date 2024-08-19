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
    }

    public override void OnExit()
    {
        base.OnExit();
        player.input.inputHandle.Character.Enable();
        player.input.inputHandle.SexAction.Disable();
        GameManager.ResetSexEnemies();
        player.uiPlayerHint.SetResistHint(SexResistEnum.Horizontal, false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!player.IsSexing)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
