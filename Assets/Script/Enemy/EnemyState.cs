public class EnemyState : EntityState
{
    public Enemy enemy;
    public EnemyState(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
        enemy = _entity;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (enemy.IsDied)
        {
            if (FSM.currentState != enemy.dieState)
            { FSM.ChangeState(enemy.dieState); }
            return;
        }
        if (enemy.IsStunning)
        {
            if (FSM.currentState != enemy.beCounteredState && FSM.currentState != enemy.stunState)
            {
                FSM.ChangeState(enemy.beCounteredState);
            }
            return;
        }
        if (enemy.IsHurting && FSM.currentState != enemy.hurtState)
        {
            FSM.ChangeState(enemy.hurtState);
            return;
        }
    }
}
