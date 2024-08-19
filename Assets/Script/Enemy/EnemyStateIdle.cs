public class EnemyStateIdle : EnemyState
{
    public EnemyStateIdle(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        stateTime = 0.2f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateTime < 0 && enemy.IsAlerting)
        {
            FSM.ChangeState(enemy.alertState);
            return;
        }
    }
}
