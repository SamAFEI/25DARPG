using UnityEngine;

public class EnemyStateDie : EnemyState
{
    public EnemyStateDie(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateTime = 3f;
        enemy.SetZeroVelocity();
        enemy.IgnoreLayersTrigger(1);
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        enemy.SetZeroVelocity();
        if (isAnimFinish)
        {
            enemy.Die();
        }
    }
}
