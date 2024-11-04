public class EnemyStateBeCountered : EnemyState
{
    public EnemyStateBeCountered(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
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
        if (isAnimFinish)
        {
            FSM.SetNextState(enemy.stunState);
            return;
        }
    }
}