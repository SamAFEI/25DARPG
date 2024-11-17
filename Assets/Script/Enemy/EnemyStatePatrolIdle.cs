public class EnemyStatePatrolIdle : EnemyState
{
    public EnemyStatePatrolIdle(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateTime = 2f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (enemy.IsAlerting)
        {
            FSM.SetNextState(enemy.alertState);
            return;
        }
        if (stateTime < 0)
        {
            FSM.SetNextState(enemy.patrolMoveState);
            return;
        }
    }

}
