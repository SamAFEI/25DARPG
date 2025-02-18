using UnityEngine;

public class PlayerStateSword : PlayerStateGrounded
{
    protected int attackCount;
    protected float lastTimeAttacked;
    protected float attackWindow = 0.2f;
    public PlayerStateSword(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        animName = "Sword1";
        int maxCombots = 2;
        if (attackCount > maxCombots || Time.time >= lastTimeAttacked + attackWindow)
        {
            attackCount = 0;
        }
        attackCount++;
        base.OnEnter();
        player.SetZeroVelocity();
        player.AttackDamage = player.Data.AttackDamage;
        if (attackCount == 3) 
        { 
            player.IsHeaveyAttack = true;
            player.AttackDamage = player.Data.AttackDamage * 1.5f;
        }
    }
    public override void OnExit()
    {
        base.OnExit();
        lastTimeAttacked = Time.time;
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

    public override void AnimatorPlay()
    {
        animName = "Sword" + attackCount;
        base.AnimatorPlay();
    }
}
