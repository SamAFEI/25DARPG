
using UnityEngine;

public class PlayerStateEarthshatter : PlayerStateGrounded
{
    public PlayerStateEarthshatter(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        player.SetZeroVelocity();
        player.IsHeaveyAttack = true;
        player.AttackDamage = player.Data.EarthshatterDamage;
    }
    public override void OnExit()
    {
        base.OnExit();
        player.SetZeroVelocity();
        player.input.SetAttacking(false);
        player.IsHeaveyAttack = false;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isAnimFinish)
        {
            FSM.SetNextState(player.idleState);
            return;
        }
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (player.IsMoveToTarget)
        {
            player.DoAttactMove(2f);
        }
    }
}
