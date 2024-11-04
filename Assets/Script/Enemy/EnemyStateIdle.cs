public class EnemyStateIdle : EnemyState
{
    public EnemyStateIdle(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
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
}
