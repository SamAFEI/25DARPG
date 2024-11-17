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
            FSM.SetNextState(enemy.dieState);
            return;
        }
        if (enemy.IsStunning && FSM.currentState != enemy.stunState)
        {
            FSM.SetNextState(enemy.beCounteredState);
            return;
        }
        if (enemy.IsHurting && !enemy.IsStunning)
        {
            FSM.SetNextState(enemy.hurtState);
            return;
        }
    }
}
