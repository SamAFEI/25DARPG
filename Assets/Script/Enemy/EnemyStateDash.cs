using UnityEngine;

public class EnemyStateDash : EnemyState
{
    public EnemyStateDash(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.ChaseSpeed = 1.6f;
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.ChaseSpeed = 1f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (enemy.CheckDashAttack())
        {
            enemy.DashAttack();
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        enemy.DoChase();
    }

}
