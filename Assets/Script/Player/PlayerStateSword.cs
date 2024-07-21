using UnityEngine;

public class PlayerStateSword : PlayerStateGrounded
{
    protected int attackCount;
    public PlayerStateSword(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        animName = "Sword1";
        base.OnEnter();
        player.SetZeroVelocity();
        attackCount = 1;
    }
    public override void OnExit()
    {
        base.OnExit();
        player.input.SetAttacking(false);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isAnimFinish)
        {
            if (player.input.IsPressedAttack && attackCount < 3)
            {
                attackCount++;
                animName = "Sword" + attackCount;
                isAnimFinish = false;
                AnimatorPlay();
                return;
            }
            FSM.ChangeState(player.idleState);
            return;
        }
    }

    public override void AnimatorPlay()
    {
        base.AnimatorPlay();
    }
}
