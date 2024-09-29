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
        if (attackCount > 2 || Time.time >= lastTimeAttacked + attackWindow)
        {
            attackCount = 0;
        }
        attackCount++;
        base.OnEnter();
        player.SetZeroVelocity();
        if (attackCount == 3) { player.IsHeaveyAttack = true; }
    }
    public override void OnExit()
    {
        base.OnExit();
        lastTimeAttacked = Time.time;
        player.input.SetAttacking(false);
        player.IsHeaveyAttack = false;
        player.IsCounter = false;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isAnimFinish)
        {
            FSM.ChangeState(player.idleState);
            return;
        }
    }

    public override void AnimatorPlay()
    {
        animName = "Sword" + attackCount;
        base.AnimatorPlay();
    }
}
