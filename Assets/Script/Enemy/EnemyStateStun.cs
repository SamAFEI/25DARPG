public class EnemyStateStun : EnemyState
{
    public EnemyStateStun(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        enemy.entityFX.DoPlayBuffFX(0, 2f);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!enemy.IsStunning)
        {
            FSM.SetNextState(enemy.idleState);
            return;
        }
    }
}

