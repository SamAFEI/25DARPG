using UnityEngine;

public class EnemyStateDash : EnemyState
{
    public EnemyStateDash(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.ChaseSpeed = enemy.Data.dashSpeed;
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.ChaseSpeed = 1f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!enemy.CanChase)
        {
            FSM.SetNextState(enemy.alertState);
            return;
        }
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
