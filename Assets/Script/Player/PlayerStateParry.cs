using UnityEngine;

public class PlayerStateParry : PlayerState
{
    protected bool canCounter;
    public PlayerStateParry(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
        player.SetDefendMeshActive(true);
        player.IsCounter = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        player.input.SetParrying(false);
        player.SetDefendMeshActive(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (player.IsCounter)
        {
            FSM.ChangeState(player.swordState);
            return;
        }
        if (player.IsHurting && player.IsStunning)
        {
            FSM.ChangeState(player.hurtState); 
            return;
        }
        if (!player.input.IsParrying)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }
}
