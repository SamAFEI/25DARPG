public class EnemyStatePatrolMove : EnemyState
{
    public EnemyStatePatrolMove(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (enemy.IsAlerting)
        {
            FSM.SetNextState(enemy.alertState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (enemy.ArriveaPatrolPoing())
        {
            FSM.SetNextState(enemy.patrolIdleState);
            return;
        }
        enemy.DoPatrol();
    }
}
